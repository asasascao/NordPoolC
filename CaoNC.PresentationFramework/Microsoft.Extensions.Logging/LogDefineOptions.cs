namespace CaoNC.Microsoft.Extensions.Logging
{

    /// <summary>
    /// Options for <see cref="M:Microsoft.Extensions.Logging.LoggerMessage.Define(Microsoft.Extensions.Logging.LogLevel,Microsoft.Extensions.Logging.EventId,System.String)" /> and its overloads
    /// </summary>
    public class LogDefineOptions
    {
        /// <summary>
        /// Gets or sets the flag to skip IsEnabled check for the logging method.
        /// </summary>
        public bool SkipEnabledCheck { get; set; }
    }
}
