namespace NordPoolC.Enums
{
    public enum PublishingMode
    {
        /// <summary>
        /// Published all outgoing messages as soon as they appear. 
        /// 流
        /// </summary>
        STREAMING,

        /// <summary>
        /// Aggregates messages for given time interval publishing only latest versions.
        /// 合并
        /// </summary>
        CONFLATED
    }
}
