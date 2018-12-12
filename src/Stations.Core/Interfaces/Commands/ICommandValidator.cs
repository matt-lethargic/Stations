using FluentValidation.Results;

namespace Stations.Core.Interfaces.Commands
{
    public interface ICommandValidator<ICommand>
    {
        ValidationResult ValidateCommand(ICommand command);
    }
}
