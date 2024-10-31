namespace CaoNC.Microsoft.Extensions.Options
{
    public interface IOptionsSnapshot<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TOptions> : IOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// Returns a configured <typeparamref name="TOptions" /> instance with the given <paramref name="name" />.
        /// </summary>
        /// <param name="name">The name of the <typeparamref name="TOptions" /> instance, if <see langword="null" /> <see cref="F:Microsoft.Extensions.Options.Options.DefaultName" /> is used.</param>
        /// <returns>The <typeparamref name="TOptions" /> instance that matches the given <paramref name="name" />.</returns>
        TOptions Get(string name);
    }
}
