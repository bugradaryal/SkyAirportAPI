using DataAccess.Abstract;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Utilitys.Logging.ExceptionHandler;


namespace Business.Features.AircraftStatus.Commands
{
    public class AddAircraftStatusHandler : IRequestHandler<AddAircraftStatusRequest>
    {
        private readonly IGenericRepository<Entities.AircraftStatus> _genericRepository;
        public readonly IAircraftStatusRepository _aircraftStatusRepository;
        public AddAircraftStatusHandler(IGenericRepository<Entities.AircraftStatus> genericRepository, IAircraftStatusRepository aircraftStatusRepository)
        {
            _genericRepository = genericRepository;
            _aircraftStatusRepository = aircraftStatusRepository;
        }

        public async Task Handle(AddAircraftStatusRequest request, CancellationToken cancellationToken)
        {
            if (!await _aircraftStatusRepository.AnyStatus(request.newStatus))
                await _genericRepository.Add(new Entities.AircraftStatus { Status = request.newStatus });
            else
                throw new CustomException("Status Allready Exsist!!", (int)HttpStatusCode.BadRequest);
        }
    }
}
