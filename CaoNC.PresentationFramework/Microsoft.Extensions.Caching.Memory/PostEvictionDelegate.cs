namespace CaoNC.Microsoft.Extensions.Caching.Memory
{
    public delegate void PostEvictionDelegate(object key, object value, EvictionReason reason, object state);
}
