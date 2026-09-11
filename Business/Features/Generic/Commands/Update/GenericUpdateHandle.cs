using DataAccess.Abstract;
using MediatR;
using System.Net;
using Utilitys.Logging.ExceptionHandler;

namespace Business.Features.Generic.Commands.Update
{
    public class GenericUpdateHandle<TEntity> : IRequestHandler<GenericUpdateRequest<TEntity>> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericUpdateHandle(IGenericRepository<TEntity> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task Handle(GenericUpdateRequest<TEntity> request, CancellationToken cancellationToken)
        {
            var entity = request.ToEntity();
            var idValue = entity.GetType().GetProperty("id")?.GetValue(entity);
            if (idValue is not int entityId || entityId <= 0)
                throw new CustomException("Id is invalid!!", (int)HttpStatusCode.BadRequest);
            if (!await _genericRepository.Any(entityId))
                throw new CustomException("Id not matched!!", (int)HttpStatusCode.NotFound);
            await _genericRepository.Update(entity);
        }
    }
}
