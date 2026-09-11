using MediatR;

namespace Business.Features.Generic.Queries.GetById
{
    public record GenericGetByIdRequest<TEntity>(int objectId) : IRequest<GenericGetByIdResponse<TEntity>>;
}
