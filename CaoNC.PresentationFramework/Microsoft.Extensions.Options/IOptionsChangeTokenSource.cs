using CaoNC.Microsoft.Extensions.Primitives;

namespace CaoNC.Microsoft.Extensions.Options
{
    public interface IOptionsChangeTokenSource<out TOptions>
    {
        /// <summary>
        /// The name of the option instance being changed.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns a <see cref="T:Microsoft.Extensions.Primitives.IChangeToken" /> which can be used to register a change notification callback.
        /// </summary>
        /// <returns>Change token.</returns>
        IChangeToken GetChangeToken();
    }
}
