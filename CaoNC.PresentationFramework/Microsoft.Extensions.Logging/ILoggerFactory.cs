using System;

namespace CaoNC.Microsoft.Extensions.Logging
{

	/// <summary>
	/// Represents a type used to configure the logging system and create instances of <see cref="T:Microsoft.Extensions.Logging.ILogger" /> from
	/// the registered <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" />s.
	/// </summary>
	public interface ILoggerFactory : IDisposable
	{
		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
		/// </summary>
		/// <param name="categoryName">The category name for messages produced by the logger.</param>
		/// <returns>The <see cref="T:Microsoft.Extensions.Logging.ILogger" />.</returns>
		ILogger CreateLogger(string categoryName);

		/// <summary>
		/// Adds an <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" /> to the logging system.
		/// </summary>
		/// <param name="provider">The <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" />.</param>
		void AddProvider(ILoggerProvider provider);
	}
}
