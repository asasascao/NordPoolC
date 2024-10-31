using System;
using System.Threading;

namespace CaoNC.Microsoft.Extensions.Logging
{

	/// <summary>
	/// Default implementation of <see cref="T:Microsoft.Extensions.Logging.IExternalScopeProvider" />
	/// </summary>
	public class LoggerExternalScopeProvider : IExternalScopeProvider
	{
		private sealed class Scope : IDisposable
		{
			private readonly LoggerExternalScopeProvider _provider;

			private bool _isDisposed;

			public Scope Parent { get; }

			public object State { get; }

			internal Scope(LoggerExternalScopeProvider provider, object state, Scope parent)
			{
				_provider = provider;
				State = state;
				Parent = parent;
			}

			public override string ToString()
			{
				return State?.ToString();
			}

			public void Dispose()
			{
				if (!_isDisposed)
				{
					lock (_provider.sock)
					{
						_provider._currentScope = Parent;
						_isDisposed = true;
					}
				}
			}
		}

		private Scope _currentScope=null;
		private object sock = new object();


		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Extensions.Logging.LoggerExternalScopeProvider" />.
		/// </summary>
		public LoggerExternalScopeProvider()
		{
		}

		/// <inheritdoc />
		public void ForEachScope<TState>(Action<object, TState> callback, TState state)
		{
			Action<object, TState> callback2 = callback;
			TState state2 = state;
			if (_currentScope != null)
			{
				lock (sock)
				{
					Report(_currentScope);
				}
			}
			void Report(Scope current)
			{
				if (current != null)
				{
					Report(current.Parent);
					callback2(current.State, state2);
				}
			}
		}

		/// <inheritdoc />
		public IDisposable Push(object state)
		{
			lock (sock)
			{
				Scope value = _currentScope;
				Scope scope = new Scope(this, state, value);
				_currentScope = scope;
				return scope;
			}
		}
	}
}
