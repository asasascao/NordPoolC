using System;
using System.Diagnostics;
using System.Text;

namespace CaoNC.Microsoft.Extensions.Logging
{
	/// <summary>
	/// Delegates to a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance using the full name of the given type, created by the
	/// provided <see cref="T:Microsoft.Extensions.Logging.ILoggerFactory" />.
	/// </summary>
	/// <typeparam name="T">The type.</typeparam>
	[DebuggerDisplay("{DebuggerToString(),nq}")]
	public class Logger<T> : ILogger<T>, ILogger
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private readonly ILogger _logger;

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Extensions.Logging.Logger`1" />.
		/// </summary>
		/// <param name="factory">The factory.</param>
		public Logger(ILoggerFactory factory)
		{
			System.ThrowHelper.ThrowIfNull(factory, "factory");
			_logger = factory.CreateLogger(GetCategoryName());
		}

		/// <inheritdoc />
		IDisposable ILogger.BeginScope<TState>(TState state)
		{
			return _logger.BeginScope(state);
		}

		/// <inheritdoc />
		bool ILogger.IsEnabled(LogLevel logLevel)
		{
			return _logger.IsEnabled(logLevel);
		}

		/// <inheritdoc />
		void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			_logger.Log(logLevel, eventId, state, exception, formatter);
		}

		private static string GetCategoryName()
		{
			return TypeNameHelper.GetTypeDisplayName(typeof(T), fullName: true, includeGenericParameterNames: false, includeGenericParameters: false, '.');
		}

		internal string DebuggerToString()
		{
			return DebuggerDisplayFormatting.DebuggerToString(GetCategoryName(), this);
		}
    }
}
