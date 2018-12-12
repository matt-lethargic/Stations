using System;
using System.Threading.Tasks;
using SimpleInjector;
using Stations.Core.Interfaces.Commands;

namespace Stations.Infrastructure
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Container _container;

        public CommandDispatcher(Container container)
        {
            _container = container;
        }

        public async Task Dispatch(ICommand command)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var wrapperType = typeof(CommandHandler<>).MakeGenericType(command.GetType());

            var handler = _container.GetInstance(handlerType);
            var wrappedHandler = (CommandHandler)Activator.CreateInstance(wrapperType, handler);

            await wrappedHandler.Handle(command);
        }


        private abstract class CommandHandler
        {
            public abstract Task Handle(ICommand command);
        }


        private class CommandHandler<T> : CommandHandler
            where T : ICommand
        {
            private readonly ICommandHandler<T> _handler;

            public CommandHandler(ICommandHandler<T> handler)
            {
                _handler = handler;
            }

            public override Task Handle(ICommand command)
            {
                return _handler.Handle((T)command);
            }
        }
    }
}
