using System;
using Stations.Core.Interfaces.Events;

namespace Stations.Core.SharedKernel
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
