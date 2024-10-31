namespace CaoNC.Microsoft.Extensions.Options
{
    public static class Options
    {
        internal const System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes DynamicallyAccessedMembers = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor;

        /// <summary>
        /// The default name used for options instances: "".
        /// </summary>
        public static readonly string DefaultName = string.Empty;

        /// <summary>
        /// Creates a wrapper around an instance of <typeparamref name="TOptions" /> to return itself as an <see cref="T:Microsoft.Extensions.Options.IOptions`1" />.
        /// </summary>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <param name="options">Options object.</param>
        /// <returns>Wrapped options object.</returns>
        public static IOptions<TOptions> Create<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(TOptions options) where TOptions : class
        {
            return new OptionsWrapper<TOptions>(options);
        }
    }

}
