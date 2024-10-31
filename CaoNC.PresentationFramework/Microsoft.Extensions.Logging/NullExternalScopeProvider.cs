using System;

namespace CaoNC.Microsoft.Extensions.Logging
{

	/// <summary>
	/// Scope provider that does nothing.
	/// </summary>
	internal sealed class NullExternalScopeProvider : IExternalScopeProvider
	{
		/// <summary>
		/// Returns a cached instance of <see cref="T:Microsoft.Extensions.Logging.NullExternalScopeProvider" />.
		/// </summary>
		public static IExternalScopeProvider Instance { get; } = new NullExternalScopeProvider();


		private NullExternalScopeProvider()
		{
		}

		/// <inheritdoc />
		void IExternalScopeProvider.ForEachScope<TState>(Action<object, TState> callback, TState state)
		{
		}

		/// <inheritdoc />
		IDisposable IExternalScopeProvider.Push(object state)
		{
			return NullScope.Instance;
		}
	}
}
