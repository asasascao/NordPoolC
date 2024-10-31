using System;

namespace CaoNC.Microsoft.Extensions.Logging.Abstractions
{
	/// <summary>
	/// Minimalistic logger that does nothing.
	/// </summary>
	public class NullLogger : ILogger
	{
		/// <summary>
		/// Returns the shared instance of <see cref="T:Microsoft.Extensions.Logging.Abstractions.NullLogger" />.
		/// </summary>
		public static NullLogger Instance { get; } = new NullLogger();


		/// <summary>
		/// Initializes a new instance of the <see cref="T:Microsoft.Extensions.Logging.Abstractions.NullLogger" /> class.
		/// </summary>
		private NullLogger()
		{
		}

		/// <inheritdoc />
		public IDisposable BeginScope<TState>(TState state)
		{
			return NullScope.Instance;
		}

		/// <inheritdoc />
		public bool IsEnabled(LogLevel logLevel)
		{
			return false;
		}

		/// <inheritdoc />
		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
		}
	}
	/// <summary>
	/// Minimalistic logger that does nothing.
	/// </summary>
	public class NullLogger<T> : ILogger<T>, ILogger
	{
		/// <summary>
		/// Returns an instance of <see cref="T:Microsoft.Extensions.Logging.Abstractions.NullLogger`1" />.
		/// </summary>
		/// <returns>An instance of <see cref="T:Microsoft.Extensions.Logging.Abstractions.NullLogger`1" />.</returns>
		public static readonly NullLogger<T> Instance = new NullLogger<T>();

		/// <inheritdoc />
		public IDisposable BeginScope<TState>(TState state)
		{
			return NullScope.Instance;
		}

		/// <inheritdoc />
		/// <remarks>
		/// This method ignores the parameters and does nothing.
		/// </remarks>
		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
		}

		/// <inheritdoc />
		public bool IsEnabled(LogLevel logLevel)
		{
			return false;
		}
	}
}
