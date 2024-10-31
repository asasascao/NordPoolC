using CaoNC.System.Diagnostics.CodeAnalysis;

namespace CaoNC.Microsoft.Extensions.Options
{
    public interface IOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TOptions> where TOptions : class
    {
        /// <summary>
        /// The default configured <typeparamref name="TOptions" /> instance
        /// </summary>
        TOptions Value { get; }
    }
}
