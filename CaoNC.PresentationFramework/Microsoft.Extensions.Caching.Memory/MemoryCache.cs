using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CaoNC.Microsoft.Extensions.Logging;
using CaoNC.Microsoft.Extensions.Logging.Abstractions;
using CaoNC.Microsoft.Extensions.Options;
using CaoNC.System;
using CaoNC.System.Diagnostics.CodeAnalysis;
using CaoNC.System.Marvin;
using CaoNC.System.Memory;

namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public class MemoryCache : IMemoryCache, IDisposable
    {
        internal sealed class Stats
        {
            private readonly MemoryCache _memoryCache;

            public long Hits;

            public long Misses;

            public Stats()
            {
            }

            public Stats(MemoryCache memoryCache)
            {
                _memoryCache = memoryCache;
                _memoryCache.AddToStats(this);
            }

            ~Stats()
            {
                _memoryCache?.RemoveFromStats(this);
            }
        }

        /// <summary>
        /// Wrapper for the memory cache entries collection.
        ///
        /// Entries may have various sizes. If a size limit has been set, the cache keeps track of the aggregate of all the entries' sizes
        /// in order to trigger compaction when the size limit is exceeded.
        ///
        /// For performance reasons, the size is not updated atomically with the collection, but is only made eventually consistent.
        ///
        /// When the memory cache is cleared, it replaces the backing collection entirely. This may occur in parallel with operations
        /// like add, set, remove, and compact which may modify the collection and thus its overall size.
        ///
        /// To keep the overall size eventually consistent, therefore, the collection and the overall size are wrapped in this CoherentState
        /// object. Individual operations take a local reference to this wrapper object while they work, and make size updates to this object.
        /// Clearing the cache simply replaces the object, so that any still in progress updates do not affect the overall size value for
        /// the new backing collection.
        /// </summary>
        private sealed class CoherentState
        {
            private sealed class StringKeyComparer : IEqualityComparer<string>, IEqualityComparer
            {
                internal static readonly IEqualityComparer<string> Instance = new StringKeyComparer();

                private StringKeyComparer()
                {
                }

                public int GetHashCode(string s)
                {
                    if (s != null)
                    {
                        return Marvin.ComputeHash32(MemoryMarshal.AsBytes(s.AsSpan()), Marvin.DefaultSeed);
                    }
                    return 0;
                }

                public bool Equals(string x, string y)
                {
                    return string.Equals(x, y);
                }

                bool IEqualityComparer.Equals(object x, object y)
                {
                    return object.Equals(x, y);
                }

                int IEqualityComparer.GetHashCode(object obj)
                {
                    if (!(obj is string s))
                    {
                        return 0;
                    }
                    return GetHashCode(s);
                }
            }

            private readonly ConcurrentDictionary<string, CacheEntry> _stringEntries = new ConcurrentDictionary<string, CacheEntry>(StringKeyComparer.Instance);

            private readonly ConcurrentDictionary<object, CacheEntry> _nonStringEntries = new ConcurrentDictionary<object, CacheEntry>();

            internal int _cacheSize;

            private ICollection<KeyValuePair<string, CacheEntry>> StringEntriesCollection => _stringEntries;

            private ICollection<KeyValuePair<object, CacheEntry>> NonStringEntriesCollection => _nonStringEntries;

            internal int Count => _stringEntries.Count + _nonStringEntries.Count;

            internal long Size => Volatile.Read(ref _cacheSize);

            internal bool TryGetValue(object key, out CacheEntry entry)
            {
                if (!(key is string key2))
                {
                    return _nonStringEntries.TryGetValue(key, out entry);
                }
                return _stringEntries.TryGetValue(key2, out entry);
            }

            internal bool TryRemove(object key, out CacheEntry entry)
            {
                if (!(key is string key2))
                {
                    return _nonStringEntries.TryRemove(key, out entry);
                }
                return _stringEntries.TryRemove(key2, out entry);
            }

            internal bool TryAdd(object key, CacheEntry entry)
            {
                if (!(key is string key2))
                {
                    return _nonStringEntries.TryAdd(key, entry);
                }
                return _stringEntries.TryAdd(key2, entry);
            }

            internal bool TryUpdate(object key, CacheEntry entry, CacheEntry comparison)
            {
                if (!(key is string key2))
                {
                    return _nonStringEntries.TryUpdate(key, entry, comparison);
                }
                return _stringEntries.TryUpdate(key2, entry, comparison);
            }

            public IEnumerable<CacheEntry> GetAllValues()
            {
                foreach (KeyValuePair<string, CacheEntry> stringEntry in _stringEntries)
                {
                    yield return stringEntry.Value;
                }
                foreach (KeyValuePair<object, CacheEntry> nonStringEntry in _nonStringEntries)
                {
                    yield return nonStringEntry.Value;
                }
            }

            internal void RemoveEntry(CacheEntry entry, MemoryCacheOptions options)
            {
                if (entry.Key is string key)
                {
                    if (StringEntriesCollection.Remove(new KeyValuePair<string, CacheEntry>(key, entry)))
                    {
                        if (options.SizeLimit.HasValue)
                        {
                            Interlocked.Add(ref _cacheSize, -entry.Size1);
                        }
                        entry.InvokeEvictionCallbacks();
                    }
                }
                else if (NonStringEntriesCollection.Remove(new KeyValuePair<object, CacheEntry>(entry.Key, entry)))
                {
                    if (options.SizeLimit.HasValue)
                    {
                        Interlocked.Add(ref _cacheSize, -entry.Size1);
                    }
                    entry.InvokeEvictionCallbacks();
                }
            }
        }

        internal readonly ILogger _logger;

        private readonly MemoryCacheOptions _options;

        private readonly List<WeakReference<Stats>> _allStats;

        private readonly Stats _accumulatedStats;

        private readonly ThreadLocal<Stats> _stats;

        private CoherentState _coherentState;

        private bool _disposed;

        private DateTime _lastExpirationScan;

        private DateTime UtcNow => _options.Clock?.UtcNow.UtcDateTime ?? DateTime.UtcNow;

        /// <summary>
        /// Gets the count of the current entries for diagnostic purposes.
        /// </summary>
        public int Count => _coherentState.Count;

        /// <summary>
        /// Internal accessor for Size for testing only.
        ///
        /// Note that this is only eventually consistent with the contents of the collection.
        /// See comment on <see cref="T:Microsoft.Extensions.Caching.Memory.MemoryCache.CoherentState" />.
        /// </summary>
        internal long Size => _coherentState.Size;

        internal bool TrackLinkedCacheEntries { get; }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Caching.Memory.MemoryCache" /> instance.
        /// </summary>
        /// <param name="optionsAccessor">The options of the cache.</param>
        public MemoryCache(IOptions<MemoryCacheOptions> optionsAccessor)
            : this(optionsAccessor, (ILoggerFactory)(object)NullLoggerFactory.Instance)
        {
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Caching.Memory.MemoryCache" /> instance.
        /// </summary>
        /// <param name="optionsAccessor">The options of the cache.</param>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        public MemoryCache(IOptions<MemoryCacheOptions> optionsAccessor, ILoggerFactory loggerFactory)
        {
            System.ThrowHelper.ThrowIfNull(optionsAccessor, "optionsAccessor");
            System.ThrowHelper.ThrowIfNull(loggerFactory, "loggerFactory");
            _options = optionsAccessor.Value;
            _logger = (ILogger)(object)LoggerFactoryExtensions.CreateLogger<MemoryCache>(loggerFactory);
            _coherentState = new CoherentState();
            if (_options.TrackStatistics)
            {
                _allStats = new List<WeakReference<Stats>>();
                _accumulatedStats = new Stats();
                _stats = new ThreadLocal<Stats>(() => new Stats(this));
            }
            _lastExpirationScan = UtcNow;
            TrackLinkedCacheEntries = _options.TrackLinkedCacheEntries;
        }

        /// <summary>
        /// Cleans up the background collection events.
        /// </summary>
        ~MemoryCache()
        {
            Dispose(disposing: false);
        }

        /// <inheritdoc />
        public ICacheEntry CreateEntry(object key)
        {
            CheckDisposed();
            ValidateCacheKey(key);
            return (ICacheEntry)(object)new CacheEntry(key, this);
        }

        internal void SetEntry(CacheEntry entry)
        {
            if (_disposed)
            {
                return;
            }
            if (_options.HasSizeLimit && entry.Size < 0)
            {
                throw new InvalidOperationException(System.SR.Format(System.SR.CacheEntryHasEmptySize, "Size", "SizeLimit"));
            }
            DateTime utcNow = UtcNow;
            if (entry.AbsoluteExpirationRelativeToNow1.Ticks > 0)
            {
                long ticks = (utcNow + entry.AbsoluteExpirationRelativeToNow1).Ticks;
                if ((ulong)ticks < (ulong)entry.AbsoluteExpirationTicks)
                {
                    entry.AbsoluteExpirationTicks = ticks;
                }
            }
            entry.LastAccessed = utcNow;
            CoherentState coherentState = _coherentState;
            if (coherentState.TryGetValue(entry.Key, out var entry2))
            {
                entry2.SetExpired((EvictionReason)2);
            }
            if (entry.CheckExpired(utcNow))
            {
                entry.InvokeEvictionCallbacks();
                if (entry2 != null)
                {
                    coherentState.RemoveEntry(entry2, _options);
                }
            }
            else if (!UpdateCacheSizeExceedsCapacity(entry, coherentState))
            {
                bool flag;
                if (entry2 == null)
                {
                    flag = coherentState.TryAdd(entry.Key, entry);
                }
                else
                {
                    flag = coherentState.TryUpdate(entry.Key, entry, entry2);
                    if (flag)
                    {
                        if (_options.HasSizeLimit)
                        {
                            Interlocked.Add(ref coherentState._cacheSize, -entry2.Size1);
                        }
                    }
                    else
                    {
                        flag = coherentState.TryAdd(entry.Key, entry);
                    }
                }
                if (flag)
                {
                    entry.AttachTokens();
                }
                else
                {
                    if (_options.HasSizeLimit)
                    {
                        Interlocked.Add(ref coherentState._cacheSize, -entry.Size1);
                    }
                    entry.SetExpired((EvictionReason)2);
                    entry.InvokeEvictionCallbacks();
                }
                entry2?.InvokeEvictionCallbacks();
            }
            else
            {
                entry.SetExpired((EvictionReason)5);
                TriggerOvercapacityCompaction();
                entry.InvokeEvictionCallbacks();
                if (entry2 != null)
                {
                    coherentState.RemoveEntry(entry2, _options);
                }
            }
            StartScanForExpiredItemsIfNeeded(utcNow);
        }

        /// <inheritdoc />
        public bool TryGetValue(object key, out object result)
        {
            //IL_0036: Unknown result type (might be due to invalid IL or missing references)
            //IL_003c: Invalid comparison between Unknown and I4
            System.ThrowHelper.ThrowIfNull(key, "key");
            CheckDisposed();
            DateTime utcNow = UtcNow;
            CoherentState coherentState = _coherentState;
            if (coherentState.TryGetValue(key, out var entry))
            {
                CacheEntry cacheEntry = entry;
                if (!cacheEntry.CheckExpired(utcNow) || (int)cacheEntry.EvictionReason == 2)
                {
                    cacheEntry.LastAccessed = utcNow;
                    result = cacheEntry.Value;
                    if (TrackLinkedCacheEntries)
                    {
                        cacheEntry.PropagateOptionsToCurrent();
                    }
                    StartScanForExpiredItemsIfNeeded(utcNow);
                    if (_allStats != null)
                    {
                        if (IntPtr.Size == 4)
                        {
                            Interlocked.Increment(ref GetStats().Hits);
                        }
                        else
                        {
                            GetStats().Hits++;
                        }
                    }
                    return true;
                }
                coherentState.RemoveEntry(cacheEntry, _options);
            }
            StartScanForExpiredItemsIfNeeded(utcNow);
            result = null;
            if (_allStats != null)
            {
                if (IntPtr.Size == 4)
                {
                    Interlocked.Increment(ref GetStats().Misses);
                }
                else
                {
                    GetStats().Misses++;
                }
            }
            return false;
        }

        /// <inheritdoc />
        public void Remove(object key)
        {
            System.ThrowHelper.ThrowIfNull(key, "key");
            CheckDisposed();
            CoherentState coherentState = _coherentState;
            if (coherentState.TryRemove(key, out var entry))
            {
                if (_options.HasSizeLimit)
                {
                    Interlocked.Add(ref coherentState._cacheSize, -entry.Size1);
                }
                entry.SetExpired((EvictionReason)1);
                entry.InvokeEvictionCallbacks();
            }
            StartScanForExpiredItemsIfNeeded(UtcNow);
        }

        /// <summary>
        /// Removes all keys and values from the cache.
        /// </summary>
        public void Clear()
        {
            CheckDisposed();
            CoherentState coherentState = Interlocked.Exchange(ref _coherentState, new CoherentState());
            foreach (CacheEntry allValue in coherentState.GetAllValues())
            {
                allValue.SetExpired((EvictionReason)1);
                allValue.InvokeEvictionCallbacks();
            }
        }

        /// <summary>
        /// Gets a snapshot of the current statistics for the memory cache.
        /// </summary>
        /// <returns>Returns <see langword="null" /> if statistics are not being tracked because <see cref="P:Microsoft.Extensions.Caching.Memory.MemoryCacheOptions.TrackStatistics" /> is <see langword="false" />.</returns>
        public MemoryCacheStatistics GetCurrentStatistics()
        {
            //IL_000f: Unknown result type (might be due to invalid IL or missing references)
            //IL_0014: Unknown result type (might be due to invalid IL or missing references)
            //IL_0020: Unknown result type (might be due to invalid IL or missing references)
            //IL_002c: Unknown result type (might be due to invalid IL or missing references)
            //IL_0039: Unknown result type (might be due to invalid IL or missing references)
            //IL_006b: Expected O, but got Unknown
            if (_allStats != null)
            {
                Tuple<long, long> tuple = Sum();
                MemoryCacheStatistics val = new MemoryCacheStatistics();
                val.TotalMisses=tuple.Item2;
                val.TotalHits=tuple.Item1;
                val.CurrentEntryCount=(long)Count;
                val.CurrentEstimatedSize=_options.SizeLimit.HasValue ? new long?(Size) : null;
                return val;
            }
            return null;
        }

        internal void EntryExpired(CacheEntry entry)
        {
            _coherentState.RemoveEntry(entry, _options);
            StartScanForExpiredItemsIfNeeded(UtcNow);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StartScanForExpiredItemsIfNeeded(DateTime utcNow)
        {
            if (_options.ExpirationScanFrequency < utcNow - _lastExpirationScan)
            {
                ScheduleTask(utcNow);
            }
            void ScheduleTask(DateTime utcNow1)
            {
                _lastExpirationScan = utcNow1;
                Task.Factory.StartNew(delegate (object state)
                {
                    ((MemoryCache)state).ScanForExpiredItems();
                }, this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            }
        }

        private Tuple<long, long> Sum()
        {
            lock (_allStats)
            {
                long num = _accumulatedStats.Hits;
                long num2 = _accumulatedStats.Misses;
                foreach (WeakReference<Stats> allStat in _allStats)
                {
                    if (allStat.TryGetTarget(out var target))
                    {
                        num += Interlocked.Read(ref target.Hits);
                        num2 += Interlocked.Read(ref target.Misses);
                    }
                }
                return new Tuple<long, long>(num, num2);
            }
        }

        private Stats GetStats()
        {
            return _stats.Value;
        }

        private void RemoveFromStats(Stats current)
        {
            lock (_allStats)
            {
                for (int i = 0; i < _allStats.Count; i++)
                {
                    if (_allStats[i].TryGetTarget(out var target) && target == current)
                    {
                        _allStats.RemoveAt(i);
                        break;
                    }
                }
                _accumulatedStats.Hits += Interlocked.Read(ref current.Hits);
                _accumulatedStats.Misses += Interlocked.Read(ref current.Misses);
                _allStats.TrimExcess();
            }
        }

        private void AddToStats(Stats current)
        {
            lock (_allStats)
            {
                _allStats.Add(new WeakReference<Stats>(current));
            }
        }

        private void ScanForExpiredItems()
        {
            DateTime utcNow = (_lastExpirationScan = UtcNow);
            CoherentState coherentState = _coherentState;
            foreach (CacheEntry allValue in coherentState.GetAllValues())
            {
                if (allValue.CheckExpired(utcNow))
                {
                    coherentState.RemoveEntry(allValue, _options);
                }
            }
        }

        /// <summary>
        /// Returns true if increasing the cache size by the size of entry would
        /// cause it to exceed any size limit on the cache, otherwise, returns false.
        /// </summary>
        private bool UpdateCacheSizeExceedsCapacity(CacheEntry entry, CoherentState coherentState)
        {
            long sizeLimitValue = _options.SizeLimitValue;
            if (sizeLimitValue < 0)
            {
                return false;
            }
            long num = coherentState.Size;
            for (int i = 0; i < 100; i++)
            {
                long num2 = num + entry.Size1;
                if ((ulong)num2 > (ulong)sizeLimitValue)
                {
                    return true;
                }
                long num3 = Interlocked.CompareExchange(ref coherentState._cacheSize, (int)num2, (int)num);
                if (num == num3)
                {
                    return false;
                }
                num = num3;
            }
            return true;
        }

        private void TriggerOvercapacityCompaction()
        {
            if (_logger.IsEnabled((LogLevel)1))
            {
                LoggerExtensions.LogDebug(_logger, "Overcapacity compaction triggered", new object[0]);
            }
            ThreadPool.QueueUserWorkItem(delegate (object s)
            {
                ((MemoryCache)s).OvercapacityCompaction();
            }, this);
        }

        private void OvercapacityCompaction()
        {
            CoherentState coherentState = _coherentState;
            long size = coherentState.Size;
            if (_logger.IsEnabled((LogLevel)1))
            {
                LoggerExtensions.LogDebug(_logger, $"Overcapacity compaction executing. Current size {size}", new object[0]);
            }
            long sizeLimitValue = _options.SizeLimitValue;
            if (sizeLimitValue >= 0)
            {
                long num = sizeLimitValue - (long)((double)sizeLimitValue * _options.CompactionPercentage);
                if (size > num)
                {
                    Compact(size - num, (CacheEntry entry) => entry.Size1, coherentState);
                }
            }
            if (_logger.IsEnabled((LogLevel)1))
            {
                LoggerExtensions.LogDebug(_logger, $"Overcapacity compaction executed. New size {coherentState.Size}", new object[0]);
            }
        }

        /// Remove at least the given percentage (0.10 for 10%) of the total entries (or estimated memory?), according to the following policy:
        /// 1. Remove all expired items.
        /// 2. Bucket by CacheItemPriority.
        /// 3. Least recently used objects.
        /// ?. Items with the soonest absolute expiration.
        /// ?. Items with the soonest sliding expiration.
        /// ?. Larger objects - estimated by object graph size, inaccurate.
        public void Compact(double percentage)
        {
            CoherentState coherentState = _coherentState;
            int num = (int)((double)coherentState.Count * percentage);
            Compact(num, (CacheEntry _) => 1L, coherentState);
        }

        private void Compact(long removalSizeTarget, Func<CacheEntry, long> computeEntrySize, CoherentState coherentState)
        {
            //IL_0066: Unknown result type (might be due to invalid IL or missing references)
            //IL_006b: Unknown result type (might be due to invalid IL or missing references)
            //IL_006d: Unknown result type (might be due to invalid IL or missing references)
            //IL_0084: Expected I4, but got Unknown
            //IL_00de: Unknown result type (might be due to invalid IL or missing references)
            //IL_00e3: Unknown result type (might be due to invalid IL or missing references)
            List<CacheEntry> list = new List<CacheEntry>();
            List<Tuple<CacheEntry, DateTimeOffset>> list2 = new List<Tuple<CacheEntry, DateTimeOffset>>();
            List<Tuple<CacheEntry, DateTimeOffset>> list3 = new List<Tuple<CacheEntry, DateTimeOffset>>();
            List<Tuple<CacheEntry, DateTimeOffset>> list4 = new List<Tuple<CacheEntry, DateTimeOffset>>();
            long removedSize2 = 0L;
            DateTime utcNow = UtcNow;
            foreach (CacheEntry allValue in coherentState.GetAllValues())
            {
                if (allValue.CheckExpired(utcNow))
                {
                    list.Add(allValue);
                    removedSize2 += computeEntrySize(allValue);
                    continue;
                }
                CacheItemPriority priority = allValue.Priority;
                switch ((int)priority)
                {
                    case 0:
                        list2.Add(new Tuple<CacheEntry, DateTimeOffset>(allValue, allValue.LastAccessed));
                        break;
                    case 1:
                        list3.Add(new Tuple<CacheEntry, DateTimeOffset>(allValue, allValue.LastAccessed));
                        break;
                    case 2:
                        list4.Add(new Tuple<CacheEntry, DateTimeOffset>(allValue, allValue.LastAccessed));
                        break;
                    default:
                        {
                            CacheItemPriority priority2 = allValue.Priority;
                            throw new NotSupportedException("Not implemented: " + ((object)(CacheItemPriority)(priority2)).ToString());
                        }
                    case 3:
                        break;
                }
            }
            ExpirePriorityBucket(ref removedSize2, removalSizeTarget, computeEntrySize, list, list2);
            ExpirePriorityBucket(ref removedSize2, removalSizeTarget, computeEntrySize, list, list3);
            ExpirePriorityBucket(ref removedSize2, removalSizeTarget, computeEntrySize, list, list4);
            foreach (CacheEntry item2 in list)
            {
                coherentState.RemoveEntry(item2, _options);
            }
            
        }

        private static void ExpirePriorityBucket(ref long removedSize, long removalSizeTarget, Func<CacheEntry, long> computeEntrySize, List<CacheEntry> entriesToRemove, List<Tuple<CacheEntry, DateTimeOffset>> priorityEntries)
        {
            if (removalSizeTarget <= removedSize)
            {
                return;
            }
            priorityEntries.Sort((Tuple<CacheEntry, DateTimeOffset> e1,
                Tuple<CacheEntry, DateTimeOffset> e2) => e1.Item2.CompareTo(e2.Item2));
            foreach (var priorityEntry in priorityEntries)
            {
                CacheEntry item = priorityEntry.Item1;
                item.SetExpired((EvictionReason)5);
                entriesToRemove.Add(item);
                removedSize += computeEntrySize(item);
                if (removalSizeTarget <= removedSize)
                {
                    break;
                }
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(disposing: true);
        }

        /// <summary>
        /// Dispose the cache and clear all entries.
        /// </summary>
        /// <param name="disposing">Dispose the object resources if true; otherwise, take no action.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _stats?.Dispose();
                    GC.SuppressFinalize(this);
                }
                _disposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(typeof(MemoryCache).FullName);
            }
        }

        private static void ValidateCacheKey(object key)
        {
            System.ThrowHelper.ThrowIfNull(key, "key");
        }
    }
}
