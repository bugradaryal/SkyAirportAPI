using DataAccess.Abstract;
using MediatR;

namespace Business.Features.Generic.Queries.GetAll
{
    public class GenericGetAllHandler<TEntity> : IRequestHandler<GenericGetAllRequest<TEntity>, GenericGetAllResponse<TEntity>> where TEntity : class
    {
        private IGenericRepository<TEntity> _repository;
        public GenericGetAllHandler(IGenericRepository<TEntity> genericRepository)
        {
            _repository = genericRepository;
        }

        public async Task<GenericGetAllResponse<TEntity>> Handle(GenericGetAllRequest<TEntity> request, CancellationToken cancellationToken)
        {
            return new GenericGetAllResponse<TEntity> { entity = await _repository.GetAll() };
        }
    }
}
