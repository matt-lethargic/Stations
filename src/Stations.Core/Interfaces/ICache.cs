using System;

namespace Stations.Core.Interfaces
{
    public interface ICache
    {
        void Add<T>(string key, T item);

        T Get<T>(string key);

        //object Get(string key, Type type, Func<object> dataRetriever);
        //object Get(string key, Type type, Func<object> dataRetriever, TimeSpan? timeToLive);

        //Task<T> GetAsync<T>(string key, Func<Task<T>> dataRetriever) where T : class;
        //Task<T> GetAsync<T>(string key, Func<Task<T>> dataRetriever, TimeSpan? timeToLive) where T : class;

        //Task<object> GetAsync(string key, Type type, Func<Task<object>> dataRetriever);
        //Task<object> GetAsync(string key, Type type, Func<Task<object>> dataRetriever, TimeSpan? timeToLive);

        //void Remove(string key);

        TimeSpan DefaultTtl { get; }
        void Remove(string key);
    }
}
