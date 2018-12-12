using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stations.Core.SharedKernel
{
    public abstract class BaseEntity
    {
        [JsonProperty]
        public Guid Id { get; protected set; }

        [JsonIgnore]
        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}
