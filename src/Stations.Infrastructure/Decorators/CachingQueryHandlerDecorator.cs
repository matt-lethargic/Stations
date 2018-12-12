using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stations.Core.Interfaces;
using Stations.Core.Interfaces.Queries;

namespace Stations.Infrastructure.Decorators
{
    [Obsolete("Use CachingRegistryDecorator and cache lower down.")]
    public class CachingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _decoratee;
        private readonly ICache _cache;

        public CachingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decoratee, ICache cache)
        {
            _decoratee = decoratee ?? throw new ArgumentNullException(nameof(decoratee));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
        public async Task<TResult> Handle(TQuery query)
        {
            TResult result;

            string key = CreateKey(query);

            if (_cache.Get<TResult>(key) == null)
            {
                result = await _decoratee.Handle(query);
                _cache.Add(key, result);
            }
            else
            {
                result = _cache.Get<TResult>(key);
            }
            
            return result;
        }


        private string CreateKey(TQuery query)
        {
            var str = JsonConvert.SerializeObject(query);
            var bytes = Encoding.UTF8.GetBytes(str);

            HashAlgorithm hasher = SHA256.Create();
            var hash = hasher.ComputeHash(bytes);

            var sBuilder = new StringBuilder();
            foreach (var t in hash)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
