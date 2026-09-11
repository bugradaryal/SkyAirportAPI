using MediatR;

namespace Business.Features.Generic.Queries.GetAll
{
    public record GenericGetAllRequest<TEntity> : IRequest<GenericGetAllResponse<TEntity>>;
}
