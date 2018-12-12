using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleInjector;
using Stations.Core.Interfaces.Events;
using Stations.Core.SharedKernel;

namespace Stations.Infrastructure
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly Container _container;

        public DomainEventDispatcher(Container container)
        {
            _container = container;
        }

        public async Task Dispatch(BaseDomainEvent domainEvent)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var wrapperType = typeof(DomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            IEnumerable<object> handlers = _container.GetAllInstances(handlerType);


            


            var wrappedHandlers = handlers
                .Select(handler => (DomainEventHandler)Activator.CreateInstance(wrapperType, handler));

            foreach (var handler in wrappedHandlers)
            {
                await handler.Handle(domainEvent);
            }
        }

        private abstract class DomainEventHandler
        {
            public abstract Task Handle(IDomainEvent domainEvent);
        }

        private class DomainEventHandler<T> : DomainEventHandler
            where T : IDomainEvent
        {
            private readonly IDomainEventHandler<T> _handler;

            public DomainEventHandler(IDomainEventHandler<T> handler)
            {
                _handler = handler;
            }

            public override Task Handle(IDomainEvent domainEvent)
            {
                return _handler.Handle((T)domainEvent);
            }
        }
    }
}