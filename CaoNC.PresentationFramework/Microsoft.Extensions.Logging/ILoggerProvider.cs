using System;

namespace CaoNC.Microsoft.Extensions.Logging
{

	/// <summary>
	/// Represents a type that can create instances of <see cref="T:Microsoft.Extensions.Logging.ILogger" />.
	/// </summary>
	public interface ILoggerProvider : IDisposable
	{
		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
		/// </summary>
		/// <param name="categoryName">The category name for messages produced by the logger.</param>
		/// <returns>The instance of <see cref="T:Microsoft.Extensions.Logging.ILogger" /> that was created.</returns>
		ILogger CreateLogger(string categoryName);
	}
}
