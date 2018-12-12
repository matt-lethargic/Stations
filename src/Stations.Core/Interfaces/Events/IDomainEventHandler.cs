using System.Threading.Tasks;

namespace Stations.Core.Interfaces.Events
{
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        Task Handle(T domainEvent);
    }
}