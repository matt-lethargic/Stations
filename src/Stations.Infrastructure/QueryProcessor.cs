using System;
using System.Threading.Tasks;
using SimpleInjector;
using Stations.Core.Interfaces.Queries;

namespace Stations.Infrastructure
{
    public class QueryProcessor : IQueryProcessor
    {
        private readonly Container _container;

        public QueryProcessor(Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public async Task<TResult> Process<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var wrapperType = typeof(QueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            var handler = _container.GetInstance(handlerType);
            QueryHandler<TResult> wrappedHandler = (QueryHandler<TResult>) Activator.CreateInstance(wrapperType, handler);

            var result =  await wrappedHandler.Handle(query);
            return result;
        }


        private abstract class QueryHandler<TResult>
        {
            public abstract Task<TResult> Handle(IQuery<TResult> query);
        }


        private class QueryHandler<TQuery, TResult> : QueryHandler<TResult>
            where TQuery : IQuery<TResult>
        {
            private readonly IQueryHandler<TQuery, TResult> _handler;

            public QueryHandler(IQueryHandler<TQuery, TResult> handler)
            {
                _handler = handler;
            }

            public override Task<TResult> Handle(IQuery<TResult> query)
            {
                return _handler.Handle((TQuery)query);
            }
        }
    }
}
