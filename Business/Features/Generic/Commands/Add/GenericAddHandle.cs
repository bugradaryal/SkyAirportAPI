using DataAccess.Abstract;
using MediatR;


namespace Business.Features.Generic.Commands.Add
{
    public class GenericAddHandle<TEntity> : IRequestHandler<GenericAddRequest<TEntity>> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericAddHandle(IGenericRepository<TEntity> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task Handle(GenericAddRequest<TEntity> request, CancellationToken cancellationToken)
        {
            await _genericRepository.Add(request.ToEntity());
        }
    }
}
