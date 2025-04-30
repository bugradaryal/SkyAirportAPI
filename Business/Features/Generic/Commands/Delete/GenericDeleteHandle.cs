using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using DataAccess.Abstract;
using DataAccess.Concrete.Generic;
using MediatR;

namespace Business.Features.Generic.Commands.Delete
{
    public class GenericDeleteHandle<TEntity> : IRequestHandler<GenericDeleteRequest<TEntity>, CustomException> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericDeleteHandle()
        {
            _genericRepository = new GenericRepository<TEntity>();
        }

        public async Task<CustomException> Handle(GenericDeleteRequest<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                int entityId = request.objectId;
                if (entityId != null)
                {
                    if (await _genericRepository.Any((int)entityId))
                    {
                        await _genericRepository.Delete(entityId);
                        return null;
                    }
                }
                return new CustomException("Id not matched - Deleting failed!", (int)HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return new CustomException(ex.Message, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message);
            }
        }
    }
}
