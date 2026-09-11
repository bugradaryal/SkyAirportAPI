using DTO.Account;
using MediatR;


namespace Business.Features.AircraftStatus.Commands
{
    public record AddAircraftStatusRequest(string newStatus) : IRequest;
}
