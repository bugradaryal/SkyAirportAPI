using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using DataAccess.Concrete.Generic;
using MediatR;
using Utilitys.Logging.ExceptionHandler;


namespace Business.Features.Generic.Commands.Delete
{
    public class GenericDeleteHandle<TEntity> : IRequestHandler<GenericDeleteRequest<TEntity>> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericDeleteHandle(IGenericRepository<TEntity> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task Handle(GenericDeleteRequest<TEntity> request, CancellationToken cancellationToken)
        {
            int entityId = request.objectId;
            if (entityId <= 0)
                throw new CustomException("Id is null!!", (int)HttpStatusCode.BadRequest);
            else if (!await _genericRepository.Any((int)entityId))
                throw new CustomException("Id not found!!", (int)HttpStatusCode.NotFound);
            await _genericRepository.Delete(entityId);
        }
    }
}
