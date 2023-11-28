using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ShoppingCartService.Util;

public interface ICache
{
    void Add(string key, object value, TimeSpan ttl);
    void AddToCache(string resource, HttpResponseMessage response); 
    object? Get(string key); 
}

public class Cache : ICache
{
    private static readonly IDictionary<string, (DateTimeOffset, object)> cache =
        new ConcurrentDictionary<string, (DateTimeOffset, object)>();  
    
    public void Add(string key, object value, TimeSpan ttl)
    {
        // Setting the cache entry with the key, expiration time, and value
        cache[key] = (DateTimeOffset.UtcNow.Add(ttl), value);  
    }

    public void AddToCache(string resource, HttpResponseMessage response)
    {
        var cacheHeader = response.Headers.FirstOrDefault(h => h.Key == "cache-control");

        // The `out` keyword is used in the context of method parameters to indicate that a parameter is being passed by reference and that the method is expected to assign a value to it
        if (!string.IsNullOrEmpty(cacheHeader.Key) &&
            CacheControlHeaderValue.TryParse(cacheHeader.Value.ToString(), out var cacheControl) &&
            cacheControl.MaxAge.HasValue)
        {
            this.Add(resource, response, cacheControl.MaxAge.Value);
        } 
    }

    public object? Get(string resourceKey)
    {
        // Checking if the cache contains the key and the entry has not expired
        if (cache.TryGetValue(resourceKey, out var value) && value.Item1 > DateTimeOffset.UtcNow)
        {
            // Returning the cached value if it exists and has not expired
            return value.Item2; 
        }
        
        cache.Remove(resourceKey);
        return null;
    }
}