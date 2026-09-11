using MediatR;


namespace Business.Features.Generic.Commands.Update
{
    public record GenericUpdateRequest<TEntity>(TEntity Entity) : IRequest, IConvertToEntity<TEntity>
    {
        public TEntity ToEntity() => Entity;
    }
}
