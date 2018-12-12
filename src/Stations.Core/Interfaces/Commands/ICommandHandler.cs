using System.Threading.Tasks;

namespace Stations.Core.Interfaces.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}
