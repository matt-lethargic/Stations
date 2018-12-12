using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stations.Core.Interfaces;
using Stations.Core.SharedKernel;

namespace Stations.Infrastructure.Decorators
{
    public class CachingRepositoryDecorator<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IRepository<T> _decoratee;
        private readonly ICache _cache;

        public CachingRepositoryDecorator(IRepository<T> decoratee, ICache cache)
        {
            _decoratee = decoratee ?? throw new ArgumentNullException(nameof(decoratee));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Task Save(T entity)
        {
            string key = $"{typeof(T).Name}-{entity.Id}";
            _cache.Remove(key);

            string listKey = $"List-{typeof(T).Name}";
            _cache.Remove(listKey);

            return _decoratee.Save(entity);
        }

        public async Task<T> GetById(Guid id)
        {
            string key = $"{typeof(T).Name}-{id}";

            T cached = _cache.Get<T>(key);
            if (cached == null)
            {
                var entity = await _decoratee.GetById(id);
                _cache.Add(key, entity);
                return entity;
            }

            return await Task.FromResult(cached);
        }

        public async Task<IList<T>> List()
        {
            string key = $"List-{typeof(T).Name}";

            IList<T> cached = _cache.Get<IList<T>>(key);
            if (cached == null)
            {
                var result = await _decoratee.List();
                _cache.Add(key, result);
                return result;
            }

            return await Task.FromResult(cached);
        }
    }
}
