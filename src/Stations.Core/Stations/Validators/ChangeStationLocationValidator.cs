using FluentValidation;
using Stations.Core.SharedKernel;
using Stations.Core.Stations.Commands;

namespace Stations.Core.Stations.Validators
{
    public class ChangeStationLocationValidator : BaseCommandValidator<ChangeStationLocation>
    {
        public ChangeStationLocationValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty();

            RuleFor(c => c.Latitude)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.Longitude)
                .NotEmpty()
                .NotNull();
        }
    }
}