using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stations.Core.Interfaces;
using Stations.Core.Interfaces.Commands;
using Stations.Core.SharedKernel.Exceptions;
using Stations.Core.Stations.Entities;

namespace Stations.Core.Stations.Commands.Handlers
{
    public class ChangeStationLocatonCommandHandler : ICommandHandler<ChangeStationLocation>
    {
        private readonly IRepository<Station> _stationRepository;

        public ChangeStationLocatonCommandHandler(IRepository<Station> stationRepository)
        {
            _stationRepository = stationRepository ?? throw new ArgumentNullException(nameof(stationRepository));
        }

        public async Task Handle(ChangeStationLocation command)
        {
            var station = await _stationRepository.GetById(command.Id);
            if (station == null)
            {
                throw new DomainValidationException
                {
                    Errors = new Dictionary<string, string>
                    {
                        {"Id", $"No station with the Id {command.Id} exists."}
                    }
                };
            }

            station.ChangeLocation(command.Latitude, command.Longitude);

            await _stationRepository.Save(station);
        }
    }
}