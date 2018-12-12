using System.Threading.Tasks;
using FluentValidation.Results;
using Stations.Core.Interfaces.Commands;
using Stations.Core.SharedKernel.Exceptions;

namespace Stations.Infrastructure.Decorators
{
    public class ValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandValidator<TCommand> _validator;
        private readonly ICommandHandler<TCommand> _decoratee;

        public ValidationCommandHandlerDecorator(ICommandValidator<TCommand> validator, ICommandHandler<TCommand> decoratee)
        {
            _validator = validator;
            _decoratee = decoratee;
        }


        public async Task Handle(TCommand command)
        {
            ValidationResult results = _validator.ValidateCommand(command);

            if(results.IsValid)
            {
                await _decoratee.Handle(command);
            }
            else
            {
                throw new DomainValidationException(results);
            }              
        }
    }
}
