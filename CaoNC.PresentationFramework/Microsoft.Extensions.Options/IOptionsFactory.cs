namespace CaoNC.Microsoft.Extensions.Options
{
    public interface IOptionsFactory<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> where TOptions : class
    {
        /// <summary>
        /// Returns a configured <typeparamref name="TOptions" /> instance with the given <paramref name="name" />.
        /// </summary>
        /// <param name="name">The name of the <typeparamref name="TOptions" /> instance to create.</param>
        /// <returns>The created <typeparamref name="TOptions" /> instance with thw given <paramref name="name" />.</returns>
        TOptions Create(string name);
    }
}
