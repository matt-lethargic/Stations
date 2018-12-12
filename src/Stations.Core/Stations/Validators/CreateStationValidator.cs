using FluentValidation;
using Stations.Core.SharedKernel;
using Stations.Core.Stations.Commands;

namespace Stations.Core.Stations.Validators
{
    public class CreateStationValidator : BaseCommandValidator<CreateStation>
    {
        public CreateStationValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty();

            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull();
        }        
    }
}
