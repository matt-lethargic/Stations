using FluentValidation;
using FluentValidation.Results;
using Stations.Core.Interfaces.Commands;

namespace Stations.Core.SharedKernel
{
    public class BaseCommandValidator<TCommand> : AbstractValidator<TCommand>, ICommandValidator<TCommand>
        where TCommand : ICommand
    {
        public ValidationResult ValidateCommand(TCommand command)
        {
            return Validate(command);
        }
    }
}
