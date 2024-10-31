using System;

namespace CaoNC.Microsoft.Extensions.Logging.Abstractions
{

	/// <summary>
	/// An <see cref="T:Microsoft.Extensions.Logging.ILoggerFactory" /> used to create instance of
	/// <see cref="T:Microsoft.Extensions.Logging.Abstractions.NullLogger" /> that logs nothing.
	/// </summary>
	public class NullLoggerFactory : ILoggerFactory, IDisposable
	{
		/// <summary>
		/// Returns the shared instance of <see cref="T:Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory" />.
		/// </summary>
		public static readonly NullLoggerFactory Instance = new NullLoggerFactory();

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory" /> instance.
		/// </summary>
		public NullLoggerFactory()
		{
		}

		/// <inheritdoc />
		/// <remarks>
		/// This returns a <see cref="T:Microsoft.Extensions.Logging.Abstractions.NullLogger" /> instance which logs nothing.
		/// </remarks>
		public ILogger CreateLogger(string name)
		{
			return NullLogger.Instance;
		}

		/// <inheritdoc />
		/// <remarks>
		/// This method ignores the parameter and does nothing.
		/// </remarks>
		public void AddProvider(ILoggerProvider provider)
		{
		}

		/// <inheritdoc />
		public void Dispose()
		{
		}
	}
}
