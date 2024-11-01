using System;
using System.Collections;
using System.Collections.Generic;

namespace CaoNC.Microsoft.Extensions.Logging
{

	/// <summary>
	/// Creates delegates which can be later cached to log messages in a performant way.
	/// </summary>
	public static class LoggerMessage
	{
		private readonly struct LogValues : IReadOnlyList<KeyValuePair<string, object>>, IReadOnlyCollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
		{
			public static readonly Func<LogValues, Exception, string> Callback = (LogValues state, Exception exception) => state.ToString();

			private readonly LogValuesFormatter _formatter;

			public KeyValuePair<string, object> this[int index]
			{
				get
				{
					if (index == 0)
					{
						return new KeyValuePair<string, object>("{OriginalFormat}", _formatter.OriginalFormat);
					}
					throw new IndexOutOfRangeException("index");
				}
			}

			public int Count => 1;

			public LogValues(LogValuesFormatter formatter)
			{
				_formatter = formatter;
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				yield return this[0];
			}

			public override string ToString()
			{
				return _formatter.Format();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private readonly struct LogValues<T0> : IReadOnlyList<KeyValuePair<string, object>>, IReadOnlyCollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
		{
			public static readonly Func<LogValues<T0>, Exception, string> Callback = (LogValues<T0> state, Exception exception) => state.ToString();

			private readonly LogValuesFormatter _formatter;

			private readonly T0 _value0;

			public KeyValuePair<string, object> this[int index]
			{
				get
				{
					switch (index)
					{
						case 0:
							return new KeyValuePair<string, object>(_formatter.ValueNames[0], _value0);
						case 1:
							return new KeyValuePair<string, object>("{OriginalFormat}", _formatter.OriginalFormat);
						default:
							throw new IndexOutOfRangeException("index");
					}
				}
			}

			public int Count => 2;

			public LogValues(LogValuesFormatter formatter, T0 value0)
			{
				_formatter = formatter;
				_value0 = value0;
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				int i = 0;
				while (i < Count)
				{
					yield return this[i];
					int num = i + 1;
					i = num;
				}
			}

			public override string ToString()
			{
				return _formatter.Format(_value0);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private readonly struct LogValues<T0, T1> : IReadOnlyList<KeyValuePair<string, object>>, IReadOnlyCollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
		{
			public static readonly Func<LogValues<T0, T1>, Exception, string> Callback = (LogValues<T0, T1> state, Exception exception) => state.ToString();

			private readonly LogValuesFormatter _formatter;

			private readonly T0 _value0;

			private readonly T1 _value1;

			public KeyValuePair<string, object> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[1], _value1);
                        case 2:
                            return new KeyValuePair<string, object>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
                            throw new IndexOutOfRangeException("index");
                    }
                }
            }

			public int Count => 3;

			public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1)
			{
				_formatter = formatter;
				_value0 = value0;
				_value1 = value1;
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				int i = 0;
				while (i < Count)
				{
					yield return this[i];
					int num = i + 1;
					i = num;
				}
			}

			public override string ToString()
			{
				return _formatter.Format(_value0, _value1);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private readonly struct LogValues<T0, T1, T2> : IReadOnlyList<KeyValuePair<string, object>>, IReadOnlyCollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
		{
			public static readonly Func<LogValues<T0, T1, T2>, Exception, string> Callback = (LogValues<T0, T1, T2> state, Exception exception) => state.ToString();

			private readonly LogValuesFormatter _formatter;

			private readonly T0 _value0;

			private readonly T1 _value1;

			private readonly T2 _value2;

			public int Count => 4;

			public KeyValuePair<string, object> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[1], _value1);
                        case 2:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[2], _value2);
                        case 3:
                            return new KeyValuePair<string, object>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
                            throw new IndexOutOfRangeException("index");
                    }
                }
            }

			public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2)
			{
				_formatter = formatter;
				_value0 = value0;
				_value1 = value1;
				_value2 = value2;
			}

			public override string ToString()
			{
				return _formatter.Format(_value0, _value1, _value2);
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				int i = 0;
				while (i < Count)
				{
					yield return this[i];
					int num = i + 1;
					i = num;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private readonly struct LogValues<T0, T1, T2, T3> : IReadOnlyList<KeyValuePair<string, object>>, IReadOnlyCollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
		{
			public static readonly Func<LogValues<T0, T1, T2, T3>, Exception, string> Callback = (LogValues<T0, T1, T2, T3> state, Exception exception) => state.ToString();

			private readonly LogValuesFormatter _formatter;

			private readonly T0 _value0;

			private readonly T1 _value1;

			private readonly T2 _value2;

			private readonly T3 _value3;

			public int Count => 5;

			public KeyValuePair<string, object> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[1], _value1);
                        case 2:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[2], _value2);
                        case 3:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[3], _value3);
                        case 4:
                            return new KeyValuePair<string, object>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
                            throw new IndexOutOfRangeException("index");
                    }
                }
            }

			public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2, T3 value3)
			{
				_formatter = formatter;
				_value0 = value0;
				_value1 = value1;
				_value2 = value2;
				_value3 = value3;
			}

			private object[] ToArray()
			{
				return new object[4] { _value0, _value1, _value2, _value3 };
			}

			public override string ToString()
			{
				return _formatter.FormatWithOverwrite(ToArray());
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				int i = 0;
				while (i < Count)
				{
					yield return this[i];
					int num = i + 1;
					i = num;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private readonly struct LogValues<T0, T1, T2, T3, T4> : IReadOnlyList<KeyValuePair<string, object>>, IReadOnlyCollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
		{
			public static readonly Func<LogValues<T0, T1, T2, T3, T4>, Exception, string> Callback = (LogValues<T0, T1, T2, T3, T4> state, Exception exception) => state.ToString();

			private readonly LogValuesFormatter _formatter;

			private readonly T0 _value0;

			private readonly T1 _value1;

			private readonly T2 _value2;

			private readonly T3 _value3;

			private readonly T4 _value4;

			public int Count => 6;

			public KeyValuePair<string, object> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[1], _value1);
                        case 2:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[2], _value2);
                        case 3:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[3], _value3);
                        case 4:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[4], _value4);
                        case 5:
                            return new KeyValuePair<string, object>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
                            throw new IndexOutOfRangeException("index");
                    }
                }
            }

			public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2, T3 value3, T4 value4)
			{
				_formatter = formatter;
				_value0 = value0;
				_value1 = value1;
				_value2 = value2;
				_value3 = value3;
				_value4 = value4;
			}

			private object[] ToArray()
			{
				return new object[5] { _value0, _value1, _value2, _value3, _value4 };
			}

			public override string ToString()
			{
				return _formatter.FormatWithOverwrite(ToArray());
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				int i = 0;
				while (i < Count)
				{
					yield return this[i];
					int num = i + 1;
					i = num;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private readonly struct LogValues<T0, T1, T2, T3, T4, T5> : IReadOnlyList<KeyValuePair<string, object>>, IReadOnlyCollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
		{
			public static readonly Func<LogValues<T0, T1, T2, T3, T4, T5>, Exception, string> Callback = (LogValues<T0, T1, T2, T3, T4, T5> state, Exception exception) => state.ToString();

			private readonly LogValuesFormatter _formatter;

			private readonly T0 _value0;

			private readonly T1 _value1;

			private readonly T2 _value2;

			private readonly T3 _value3;

			private readonly T4 _value4;

			private readonly T5 _value5;

			public int Count => 7;

			public KeyValuePair<string, object> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[1], _value1);
                        case 2:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[2], _value2);
                        case 3:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[3], _value3);
                        case 4:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[4], _value4);
                        case 5:
                            return new KeyValuePair<string, object>(_formatter.ValueNames[5], _value5);
                        case 6:
                            return new KeyValuePair<string, object>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
                            throw new IndexOutOfRangeException("index");
                    }
                }
            }

			public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
			{
				_formatter = formatter;
				_value0 = value0;
				_value1 = value1;
				_value2 = value2;
				_value3 = value3;
				_value4 = value4;
				_value5 = value5;
			}

			private object[] ToArray()
			{
				return new object[6] { _value0, _value1, _value2, _value3, _value4, _value5 };
			}

			public override string ToString()
			{
				return _formatter.FormatWithOverwrite(ToArray());
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				int i = 0;
				while (i < Count)
				{
					yield return this[i];
					int num = i + 1;
					i = num;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		/// <summary>
		/// Creates a delegate which can be invoked to create a log scope.
		/// </summary>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log scope.</returns>
		public static Func<ILogger, IDisposable> DefineScope(string formatString)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 0);
			LogValues logValues = new LogValues(formatter);
			return (ILogger logger) => logger.BeginScope(logValues);
		}

		/// <summary>
		/// Creates a delegate which can be invoked to create a log scope.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log scope.</returns>
		public static Func<ILogger, T1, IDisposable> DefineScope<T1>(string formatString)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 1);
			return (ILogger logger, T1 arg1) => logger.BeginScope(new LogValues<T1>(formatter, arg1));
		}

		/// <summary>
		/// Creates a delegate which can be invoked to create a log scope.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log scope.</returns>
		public static Func<ILogger, T1, T2, IDisposable> DefineScope<T1, T2>(string formatString)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 2);
			return (ILogger logger, T1 arg1, T2 arg2) => logger.BeginScope(new LogValues<T1, T2>(formatter, arg1, arg2));
		}

		/// <summary>
		/// Creates a delegate which can be invoked to create a log scope.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log scope.</returns>
		public static Func<ILogger, T1, T2, T3, IDisposable> DefineScope<T1, T2, T3>(string formatString)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 3);
			return (ILogger logger, T1 arg1, T2 arg2, T3 arg3) => logger.BeginScope(new LogValues<T1, T2, T3>(formatter, arg1, arg2, arg3));
		}

		/// <summary>
		/// Creates a delegate which can be invoked to create a log scope.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log scope.</returns>
		public static Func<ILogger, T1, T2, T3, T4, IDisposable> DefineScope<T1, T2, T3, T4>(string formatString)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 4);
			return (ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => logger.BeginScope(new LogValues<T1, T2, T3, T4>(formatter, arg1, arg2, arg3, arg4));
		}

		/// <summary>
		/// Creates a delegate which can be invoked to create a log scope.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
		/// <typeparam name="T5">The type of the fifth parameter passed to the named format string.</typeparam>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log scope.</returns>
		public static Func<ILogger, T1, T2, T3, T4, T5, IDisposable> DefineScope<T1, T2, T3, T4, T5>(string formatString)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 5);
			return (ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => logger.BeginScope(new LogValues<T1, T2, T3, T4, T5>(formatter, arg1, arg2, arg3, arg4, arg5));
		}

		/// <summary>
		/// Creates a delegate which can be invoked to create a log scope.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
		/// <typeparam name="T5">The type of the fifth parameter passed to the named format string.</typeparam>
		/// <typeparam name="T6">The type of the sixth parameter passed to the named format string.</typeparam>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log scope.</returns>
		public static Func<ILogger, T1, T2, T3, T4, T5, T6, IDisposable> DefineScope<T1, T2, T3, T4, T5, T6>(string formatString)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 6);
			return (ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => logger.BeginScope(new LogValues<T1, T2, T3, T4, T5, T6>(formatter, arg1, arg2, arg3, arg4, arg5, arg6));
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, Exception> Define(LogLevel logLevel, EventId eventId, string formatString)
		{
			return Define(logLevel, eventId, formatString, null);
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <param name="options">The <see cref="T:Microsoft.Extensions.Logging.LogDefineOptions" /></param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, Exception> Define(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions options)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 0);
			if (options != null && options.SkipEnabledCheck)
			{
				return Log;
			}
			return delegate (ILogger logger, Exception exception)
			{
				if (logger.IsEnabled(logLevel))
				{
					Log(logger, exception);
				}
			};
			void Log(ILogger logger, Exception exception)
			{
				logger.Log(logLevel, eventId, new LogValues(formatter), exception, LogValues.Callback);
			}
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, Exception> Define<T1>(LogLevel logLevel, EventId eventId, string formatString)
		{
			return Define<T1>(logLevel, eventId, formatString, null);
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <param name="options">The <see cref="T:Microsoft.Extensions.Logging.LogDefineOptions" /></param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, Exception> Define<T1>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions options)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 1);
			if (options != null && options.SkipEnabledCheck)
			{
				return Log;
			}
			return delegate (ILogger logger, T1 arg1, Exception exception)
			{
				if (logger.IsEnabled(logLevel))
				{
					Log(logger, arg1, exception);
				}
			};
			void Log(ILogger logger, T1 arg1, Exception exception)
			{
				logger.Log(logLevel, eventId, new LogValues<T1>(formatter, arg1), exception, LogValues<T1>.Callback);
			}
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, T2, Exception> Define<T1, T2>(LogLevel logLevel, EventId eventId, string formatString)
		{
			return Define<T1, T2>(logLevel, eventId, formatString, null);
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <param name="options">The <see cref="T:Microsoft.Extensions.Logging.LogDefineOptions" /></param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, T2, Exception> Define<T1, T2>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions options)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 2);
			if (options != null && options.SkipEnabledCheck)
			{
				return Log;
			}
			return delegate (ILogger logger, T1 arg1, T2 arg2, Exception exception)
			{
				if (logger.IsEnabled(logLevel))
				{
					Log(logger, arg1, arg2, exception);
				}
			};
			void Log(ILogger logger, T1 arg1, T2 arg2, Exception exception)
			{
				logger.Log(logLevel, eventId, new LogValues<T1, T2>(formatter, arg1, arg2), exception, LogValues<T1, T2>.Callback);
			}
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, T2, T3, Exception> Define<T1, T2, T3>(LogLevel logLevel, EventId eventId, string formatString)
		{
			return Define<T1, T2, T3>(logLevel, eventId, formatString, null);
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <param name="options">The <see cref="T:Microsoft.Extensions.Logging.LogDefineOptions" /></param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, T2, T3, Exception> Define<T1, T2, T3>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions options)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 3);
			if (options != null && options.SkipEnabledCheck)
			{
				return Log;
			}
			return delegate (ILogger logger, T1 arg1, T2 arg2, T3 arg3, Exception exception)
			{
				if (logger.IsEnabled(logLevel))
				{
					Log(logger, arg1, arg2, arg3, exception);
				}
			};
			void Log(ILogger logger, T1 arg1, T2 arg2, T3 arg3, Exception exception)
			{
				logger.Log(logLevel, eventId, new LogValues<T1, T2, T3>(formatter, arg1, arg2, arg3), exception, LogValues<T1, T2, T3>.Callback);
			}
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, T2, T3, T4, Exception> Define<T1, T2, T3, T4>(LogLevel logLevel, EventId eventId, string formatString)
		{
			return Define<T1, T2, T3, T4>(logLevel, eventId, formatString, null);
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <param name="options">The <see cref="T:Microsoft.Extensions.Logging.LogDefineOptions" /></param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, T2, T3, T4, Exception> Define<T1, T2, T3, T4>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions options)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 4);
			if (options != null && options.SkipEnabledCheck)
			{
				return Log;
			}
			return delegate (ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Exception exception)
			{
				if (logger.IsEnabled(logLevel))
				{
					Log(logger, arg1, arg2, arg3, arg4, exception);
				}
			};
			void Log(ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Exception exception)
			{
				logger.Log(logLevel, eventId, new LogValues<T1, T2, T3, T4>(formatter, arg1, arg2, arg3, arg4), exception, LogValues<T1, T2, T3, T4>.Callback);
			}
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
		/// <typeparam name="T5">The type of the fifth parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, T2, T3, T4, T5, Exception> Define<T1, T2, T3, T4, T5>(LogLevel logLevel, EventId eventId, string formatString)
		{
			return Define<T1, T2, T3, T4, T5>(logLevel, eventId, formatString, null);
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
		/// <typeparam name="T5">The type of the fifth parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <param name="options">The <see cref="T:Microsoft.Extensions.Logging.LogDefineOptions" /></param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, T2, T3, T4, T5, Exception> Define<T1, T2, T3, T4, T5>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions options)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 5);
			if (options != null && options.SkipEnabledCheck)
			{
				return Log;
			}
			return delegate (ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Exception exception)
			{
				if (logger.IsEnabled(logLevel))
				{
					Log(logger, arg1, arg2, arg3, arg4, arg5, exception);
				}
			};
			void Log(ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Exception exception)
			{
				logger.Log(logLevel, eventId, new LogValues<T1, T2, T3, T4, T5>(formatter, arg1, arg2, arg3, arg4, arg5), exception, LogValues<T1, T2, T3, T4, T5>.Callback);
			}
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
		/// <typeparam name="T5">The type of the fifth parameter passed to the named format string.</typeparam>
		/// <typeparam name="T6">The type of the sixth parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, T2, T3, T4, T5, T6, Exception> Define<T1, T2, T3, T4, T5, T6>(LogLevel logLevel, EventId eventId, string formatString)
		{
			return Define<T1, T2, T3, T4, T5, T6>(logLevel, eventId, formatString, null);
		}

		/// <summary>
		/// Creates a delegate which can be invoked for logging a message.
		/// </summary>
		/// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
		/// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
		/// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
		/// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
		/// <typeparam name="T5">The type of the fifth parameter passed to the named format string.</typeparam>
		/// <typeparam name="T6">The type of the sixth parameter passed to the named format string.</typeparam>
		/// <param name="logLevel">The <see cref="T:Microsoft.Extensions.Logging.LogLevel" /></param>
		/// <param name="eventId">The event id</param>
		/// <param name="formatString">The named format string</param>
		/// <param name="options">The <see cref="T:Microsoft.Extensions.Logging.LogDefineOptions" /></param>
		/// <returns>A delegate which when invoked creates a log message.</returns>
		public static Action<ILogger, T1, T2, T3, T4, T5, T6, Exception> Define<T1, T2, T3, T4, T5, T6>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions options)
		{
			LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, 6);
			if (options != null && options.SkipEnabledCheck)
			{
				return Log;
			}
			return delegate (ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Exception exception)
			{
				if (logger.IsEnabled(logLevel))
				{
					Log(logger, arg1, arg2, arg3, arg4, arg5, arg6, exception);
				}
			};
			void Log(ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Exception exception)
			{
				logger.Log(logLevel, eventId, new LogValues<T1, T2, T3, T4, T5, T6>(formatter, arg1, arg2, arg3, arg4, arg5, arg6), exception, LogValues<T1, T2, T3, T4, T5, T6>.Callback);
			}
		}

		private static LogValuesFormatter CreateLogValuesFormatter(string formatString, int expectedNamedParameterCount)
		{
			LogValuesFormatter logValuesFormatter = new LogValuesFormatter(formatString);
			int count = logValuesFormatter.ValueNames.Count;
			if (count != expectedNamedParameterCount)
			{
				throw new ArgumentException(System.SR.Format(System.SR.UnexpectedNumberOfNamedParameters, formatString, expectedNamedParameterCount, count));
			}
			return logValuesFormatter;
		}
	}
}
