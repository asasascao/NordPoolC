using CaoNC.Microsoft.Extensions.Logging;
using CaoNC.Microsoft.Extensions.Primitives;
using CaoNC.System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    internal sealed class CacheEntry : ICacheEntry, IDisposable
    {
        private sealed class CacheEntryTokens
        {
            private List<IChangeToken> _expirationTokens;

            private List<IDisposable> _expirationTokenRegistrations;

            private List<PostEvictionCallbackRegistration> _postEvictionCallbacks;

            internal List<IChangeToken> ExpirationTokens => _expirationTokens ?? (_expirationTokens = new List<IChangeToken>());

            internal List<PostEvictionCallbackRegistration> PostEvictionCallbacks => _postEvictionCallbacks ?? (_postEvictionCallbacks = new List<PostEvictionCallbackRegistration>());

            internal void AttachTokens(CacheEntry cacheEntry)
            {
                List<IChangeToken> expirationTokens = _expirationTokens;
                if (expirationTokens == null)
                {
                    return;
                }
                lock (this)
                {
                    for (int i = 0; i < expirationTokens.Count; i++)
                    {
                        IChangeToken val = expirationTokens[i];
                        if (val.ActiveChangeCallbacks)
                        {
                            if (_expirationTokenRegistrations == null)
                            {
                                _expirationTokenRegistrations = new List<IDisposable>(1);
                            }
                            IDisposable item = val.RegisterChangeCallback(ExpirationCallback, (object)cacheEntry);
                            _expirationTokenRegistrations.Add(item);
                        }
                    }
                }
            }

            internal bool CheckForExpiredTokens(CacheEntry cacheEntry)
            {
                List<IChangeToken> expirationTokens = _expirationTokens;
                if (expirationTokens != null)
                {
                    for (int i = 0; i < expirationTokens.Count; i++)
                    {
                        IChangeToken val = expirationTokens[i];
                        if (val.HasChanged)
                        {
                            cacheEntry.SetExpired((EvictionReason)4);
                            return true;
                        }
                    }
                }
                return false;
            }

            internal bool CanPropagateTokens()
            {
                return _expirationTokens != null;
            }

            internal void PropagateTokens(CacheEntry parentEntry)
            {
                if (_expirationTokens == null)
                {
                    return;
                }
                lock (this)
                {
                    CacheEntryTokens orCreateTokens = parentEntry.GetOrCreateTokens();
                    lock (orCreateTokens)
                    {
                        orCreateTokens.ExpirationTokens.AddRange(_expirationTokens);
                    }
                }
            }

            internal void DetachTokens()
            {
                if (_expirationTokens == null)
                {
                    return;
                }
                lock (this)
                {
                    List<IDisposable> expirationTokenRegistrations = _expirationTokenRegistrations;
                    if (expirationTokenRegistrations != null)
                    {
                        _expirationTokenRegistrations = null;
                        for (int i = 0; i < expirationTokenRegistrations.Count; i++)
                        {
                            IDisposable disposable = expirationTokenRegistrations[i];
                            disposable.Dispose();
                        }
                    }
                }
            }

            internal void InvokeEvictionCallbacks(CacheEntry cacheEntry)
            {
                if (_postEvictionCallbacks != null)
                {
                    Task.Factory.StartNew(delegate (object state)
                    {
                        InvokeCallbacks((CacheEntry)state);
                    }, cacheEntry, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
                }
            }

            private static void InvokeCallbacks(CacheEntry entry)
            {
                //IL_003b: Unknown result type (might be due to invalid IL or missing references)
                List<PostEvictionCallbackRegistration> list = Interlocked.Exchange(ref entry._tokens._postEvictionCallbacks, null);
                if (list == null)
                {
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    PostEvictionCallbackRegistration val = list[i];
                    try
                    {
                        PostEvictionDelegate evictionCallback = val.EvictionCallback;
                        if (evictionCallback != null)
                        {
                            evictionCallback.Invoke(entry.Key, entry.Value, entry.EvictionReason, val.State);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerExtensions.LogError(entry._cache._logger, ex, "EvictionCallback invoked failed", new object[0]);
                    }
                }
            }
        }

        private static readonly Action<object> ExpirationCallback = ExpirationTokensExpired;

        private static readonly CacheEntry _current = null;
        private static object sock = new object();

        private readonly MemoryCache _cache;

        private CacheEntryTokens _tokens;

        private TimeSpan _absoluteExpirationRelativeToNow;

        private TimeSpan _slidingExpiration;

        private long _size = -1L;

        private CacheEntry _previous;

        private object _value;

        private long _absoluteExpirationTicks = -1L;

        private short _absoluteExpirationOffsetMinutes;

        private bool _isDisposed;

        private bool _isExpired;

        private bool _isValueSet;

        private byte _evictionReason;

        private byte _priority = 1;

        private const int NotSet = -1;

        internal static CacheEntry Current => _current;

        internal long AbsoluteExpirationTicks
        {
            get
            {
                return _absoluteExpirationTicks;
            }
            set
            {
                _absoluteExpirationTicks = value;
                _absoluteExpirationOffsetMinutes = 0;
            }
        }

        public DateTimeOffset? AbsoluteExpiration
        {
            get
            {
                if (_absoluteExpirationTicks < 0)
                {
                    return null;
                }
                TimeSpan offset = new TimeSpan((long)_absoluteExpirationOffsetMinutes * 600000000L);
                return new DateTimeOffset(_absoluteExpirationTicks + offset.Ticks, offset);
            }
            set
            {
                if (!value.HasValue)
                {
                    _absoluteExpirationTicks = -1L;
                    _absoluteExpirationOffsetMinutes = 0;
                }
                else
                {
                    DateTimeOffset valueOrDefault = value.GetValueOrDefault();
                    _absoluteExpirationTicks = valueOrDefault.UtcTicks;
                    _absoluteExpirationOffsetMinutes = (short)(valueOrDefault.Offset.Ticks / 600000000);
                }
            }
        }

        internal TimeSpan AbsoluteExpirationRelativeToNow1 => _absoluteExpirationRelativeToNow;

        public TimeSpan? AbsoluteExpirationRelativeToNow
        {
            get
            {
                if (_absoluteExpirationRelativeToNow.Ticks != 0L)
                {
                    return _absoluteExpirationRelativeToNow;
                }
                return null;
            }
            set
            {
                if (value.HasValue && value.GetValueOrDefault().Ticks <= 0)
                {
                    throw new ArgumentOutOfRangeException("AbsoluteExpirationRelativeToNow", value, "The relative expiration value must be positive.");
                }
                _absoluteExpirationRelativeToNow = value.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        public TimeSpan? SlidingExpiration
        {
            get
            {
                if (_slidingExpiration.Ticks != 0L)
                {
                    return _slidingExpiration;
                }
                return null;
            }
            set
            {
                if (value.HasValue && value.GetValueOrDefault().Ticks <= 0)
                {
                    throw new ArgumentOutOfRangeException("SlidingExpiration", value, "The sliding expiration value must be positive.");
                }
                _slidingExpiration = value.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Gets the <see cref="T:Microsoft.Extensions.Primitives.IChangeToken" /> instances which cause the cache entry to expire.
        /// </summary>
        [MemberNotNull("_tokens")]
        public IList<IChangeToken> ExpirationTokens
        {
            [MemberNotNull("_tokens")]
            get
            {
                return GetOrCreateTokens().ExpirationTokens;
            }
        }

        /// <summary>
        /// Gets or sets the callbacks will be fired after the cache entry is evicted from the cache.
        /// </summary>
        [MemberNotNull("_tokens")]
        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks
        {
            [MemberNotNull("_tokens")]
            get
            {
                return GetOrCreateTokens().PostEvictionCallbacks;
            }
        }

        /// <summary>
        /// Gets or sets the priority for keeping the cache entry in the cache during a
        /// memory pressure triggered cleanup. The default is <see cref="F:Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal" />.
        /// </summary>
        public CacheItemPriority Priority
        {
            get
            {
                return (CacheItemPriority)_priority;
            }
            set
            {
                //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                _priority = (byte)value;
            }
        }

        internal int Size1 => (int)_size;

        public long? Size
        {
            get
            {
                if (_size >= 0)
                {
                    return _size;
                }
                return null;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "value must be non-negative.");
                }
                _size = value ?? (-1);
            }
        }

        public object Key { get; }

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _isValueSet = true;
            }
        }

        internal DateTime LastAccessed { get; set; }

        internal EvictionReason EvictionReason
        {
            get
            {
                return (EvictionReason)_evictionReason;
            }
            private set
            {
                //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                _evictionReason = (byte)value;
            }
        }

        internal CacheEntry(object key, MemoryCache memoryCache)
        {
            System.ThrowHelper.ThrowIfNull(key, "key");
            System.ThrowHelper.ThrowIfNull(memoryCache, "memoryCache");
            Key = key;
            _cache = memoryCache;
            if (memoryCache.TrackLinkedCacheEntries)
            {
                lock (sock)
                {
                    CacheEntry current = _current;
                    _previous = current;
                    current = this;
                }
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (_cache.TrackLinkedCacheEntries)
                {
                    CommitWithTracking();
                }
                else if (_isValueSet)
                {
                    _cache.SetEntry(this);
                }
            }
        }

        private void CommitWithTracking()
        {
            _current.Value = _previous;
            if (_isValueSet)
            {
                _cache.SetEntry(this);
                CacheEntry previous = _previous;
                if (previous != null)
                {
                    if ((ulong)_absoluteExpirationTicks < (ulong)previous._absoluteExpirationTicks)
                    {
                        previous._absoluteExpirationTicks = _absoluteExpirationTicks;
                        previous._absoluteExpirationOffsetMinutes = _absoluteExpirationOffsetMinutes;
                    }
                    _tokens?.PropagateTokens(previous);
                }
            }
            _previous = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool CheckExpired(DateTime utcNow)
        {
            if (!_isExpired && !CheckForExpiredTime(utcNow))
            {
                if (_tokens != null)
                {
                    return _tokens.CheckForExpiredTokens(this);
                }
                return false;
            }
            return true;
        }

        internal void SetExpired(EvictionReason reason)
        {
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0009: Unknown result type (might be due to invalid IL or missing references)
            if ((int)EvictionReason == 0)
            {
                EvictionReason = reason;
            }
            _isExpired = true;
            _tokens?.DetachTokens();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CheckForExpiredTime(DateTime utcNow)
        {
            if (_absoluteExpirationTicks < 0 && _slidingExpiration.Ticks == 0L)
            {
                return false;
            }
            return FullCheck(utcNow);
            bool FullCheck(DateTime utcNow1)
            {
                if ((ulong)_absoluteExpirationTicks <= (ulong)utcNow1.Ticks)
                {
                    SetExpired((EvictionReason)3);
                    return true;
                }
                if (_slidingExpiration.Ticks > 0 && utcNow1 - LastAccessed >= _slidingExpiration)
                {
                    SetExpired((EvictionReason)3);
                    return true;
                }
                return false;
            }
        }

        internal void AttachTokens()
        {
            _tokens?.AttachTokens(this);
        }

        private static void ExpirationTokensExpired(object obj)
        {
            Task.Factory.StartNew(delegate (object state)
            {
                CacheEntry cacheEntry = (CacheEntry)state;
                cacheEntry.SetExpired((EvictionReason)4);
                cacheEntry._cache.EntryExpired(cacheEntry);
            }, obj, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        internal void InvokeEvictionCallbacks()
        {
            _tokens?.InvokeEvictionCallbacks(this);
        }

        internal void PropagateOptionsToCurrent()
        {
            if ((_tokens == null || !_tokens.CanPropagateTokens()) && _absoluteExpirationTicks < 0)
            {
                return;
            }
            if (_current != null)
            {
                lock (_current)
                {
                    CacheEntry value = _current;
                    if (value != null)
                    {
                        if ((ulong)_absoluteExpirationTicks < (ulong)value._absoluteExpirationTicks)
                        {
                            value._absoluteExpirationTicks = _absoluteExpirationTicks;
                            value._absoluteExpirationOffsetMinutes = _absoluteExpirationOffsetMinutes;
                        }
                        _tokens?.PropagateTokens(value);
                    }
                }
            }
        }

        [MemberNotNull("_tokens")]
        private CacheEntryTokens GetOrCreateTokens()
        {
            if (_tokens != null)
            {
                return _tokens;
            }
            CacheEntryTokens cacheEntryTokens = new CacheEntryTokens();
            return Interlocked.CompareExchange(ref _tokens, cacheEntryTokens, null) ?? cacheEntryTokens;
        }
    }
}
