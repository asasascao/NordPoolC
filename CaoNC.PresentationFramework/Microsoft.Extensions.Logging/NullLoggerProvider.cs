using System;

namespace CaoNC.Microsoft.Extensions.Logging.Abstractions
{

	/// <summary>
	/// Provider for the <see cref="T:Microsoft.Extensions.Logging.Abstractions.NullLogger" />.
	/// </summary>
	public class NullLoggerProvider : ILoggerProvider, IDisposable
	{
		/// <summary>
		/// Returns an instance of <see cref="T:Microsoft.Extensions.Logging.Abstractions.NullLoggerProvider" />.
		/// </summary>
		public static NullLoggerProvider Instance { get; } = new NullLoggerProvider();


		private NullLoggerProvider()
		{
		}

		/// <inheritdoc />
		public ILogger CreateLogger(string categoryName)
		{
			return NullLogger.Instance;
		}

		/// <inheritdoc />
		public void Dispose()
		{
		}
	}
}
