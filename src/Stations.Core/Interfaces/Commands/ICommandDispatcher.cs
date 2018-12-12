using System.Threading.Tasks;

namespace Stations.Core.Interfaces.Commands
{
    public interface ICommandDispatcher
    {
        Task Dispatch(ICommand command);
    }
}
