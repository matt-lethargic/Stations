using System.Threading.Tasks;
using Stations.Core.SharedKernel;

namespace Stations.Core.Interfaces.Events
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }
}
