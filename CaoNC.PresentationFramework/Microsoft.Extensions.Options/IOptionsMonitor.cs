using System;

namespace CaoNC.Microsoft.Extensions.Options
{
    public interface IOptionsMonitor<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TOptions>
    {
        /// <summary>
        /// Returns the current <typeparamref name="TOptions" /> instance with the <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" />.
        /// </summary>
        TOptions CurrentValue { get; }

        /// <summary>
        /// Returns a configured <typeparamref name="TOptions" /> instance with the given <paramref name="name" />.
        /// </summary>
        /// <param name="name">The name of the <typeparamref name="TOptions" /> instance, if a <see langword="null" /> <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" /> is used.</param>
        /// <returns>The <typeparamref name="TOptions" /> instance that matches the given <paramref name="name" />.</returns>
        TOptions Get(string name);

        /// <summary>
        /// Registers a listener to be called whenever a named <typeparamref name="TOptions" /> changes.
        /// </summary>
        /// <param name="listener">The action to be invoked when <typeparamref name="TOptions" /> has changed.</param>
        /// <returns>An <see cref="T:System.IDisposable" /> which should be disposed to stop listening for changes.</returns>
        IDisposable OnChange(Action<TOptions, string> listener);
    }
}
