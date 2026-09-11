using DataAccess.Abstract;
using MediatR;

namespace Business.Features.Generic.Queries.GetById
{
    public class GenericGetByIdHandler<TEntity> : IRequestHandler<GenericGetByIdRequest<TEntity>, GenericGetByIdResponse<TEntity>> where TEntity : class
    {
        private IGenericRepository<TEntity> _repository;
        public GenericGetByIdHandler(IGenericRepository<TEntity> genericRepository)
        {
            _repository = genericRepository;
        }

        public async Task<GenericGetByIdResponse<TEntity>> Handle(GenericGetByIdRequest<TEntity> request, CancellationToken cancellationToken)
        {
            return new GenericGetByIdResponse<TEntity> { entity = await _repository.GetValue(request.objectId) };
        }
    }
}
