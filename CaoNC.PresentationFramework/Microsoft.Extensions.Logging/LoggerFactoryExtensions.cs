using System;

namespace CaoNC.Microsoft.Extensions.Logging
{

	/// <summary>
	/// ILoggerFactory extension methods for common scenarios.
	/// </summary>
	public static class LoggerFactoryExtensions
	{
		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance using the full name of the given type.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <typeparam name="T">The type.</typeparam>
		/// <returns>The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> that was created.</returns>
		public static ILogger<T> CreateLogger<T>(this ILoggerFactory factory)
		{
			System.ThrowHelper.ThrowIfNull(factory, "factory");
			return new Logger<T>(factory);
		}

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance using the full name of the given <paramref name="type" />.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="type">The type.</param>
		/// <returns>The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> that was created.</returns>
		public static ILogger CreateLogger(this ILoggerFactory factory, Type type)
		{
			System.ThrowHelper.ThrowIfNull(factory, "factory");
			System.ThrowHelper.ThrowIfNull(type, "type");
			return factory.CreateLogger(TypeNameHelper.GetTypeDisplayName(type, fullName: true, includeGenericParameterNames: false, includeGenericParameters: false, '.'));
		}
	}
}
