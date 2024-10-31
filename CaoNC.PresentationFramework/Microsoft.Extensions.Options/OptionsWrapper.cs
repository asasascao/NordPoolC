namespace CaoNC.Microsoft.Extensions.Options
{
    public class OptionsWrapper<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// The options instance.
        /// </summary>
        public TOptions Value { get; }

        /// <summary>
        /// Initializes the wrapper with the options instance to return.
        /// </summary>
        /// <param name="options">The options instance to return.</param>
        public OptionsWrapper(TOptions options)
        {
            Value = options;
        }
    }
}
