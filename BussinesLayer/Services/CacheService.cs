using Microsoft.Extensions.Caching.Memory;

namespace BussinesLayer.Services;
public class CacheService<T> : ICacheService<T>
{
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache cache)
    {
        _memoryCache = cache;
    }
    public T Get(object key)
    {
        return _memoryCache.Get<T>(key);
    }
    public T Create(object key, T item)
    {
        _memoryCache.Set(key, item);

        return item;
    }


    public void Remove(object key)
    {
        _memoryCache.Remove(key);
    }
}