using MediatR;


namespace Business.Features.Generic.Commands.Add
{
    public record GenericAddRequest<TEntity>(TEntity Entity) : IRequest, IConvertToEntity<TEntity>
    {
        public TEntity ToEntity() => Entity;
    }
}
