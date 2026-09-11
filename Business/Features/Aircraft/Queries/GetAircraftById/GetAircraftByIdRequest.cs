using MediatR;

namespace Business.Features.Aircraft.Queries.GetAircraftById
{
    public record GetAircraftByIdRequest(int id) : IRequest<GetAircraftByIdResponse>;
}
