using MediatR;


namespace Business.Features.Generic.Commands.Delete
{
    public record GenericDeleteRequest<TEntity>(int objectId) : IRequest;
}
