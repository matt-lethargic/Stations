using System;
using Stations.Core.Interfaces.Commands;

namespace Stations.Core.Stations.Commands
{
    public class CreateStation : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }

        public CreateStation(Guid id, string name)
        {
            Id = id;
            Name = name;
        }               
    }
}
