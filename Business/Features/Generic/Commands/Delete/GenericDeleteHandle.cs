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
using Utilitys.ResponseHandler;

namespace Business.Features.Generic.Commands.Delete
{
    public class GenericDeleteHandle<TEntity> : IRequestHandler<GenericDeleteRequest<TEntity>, ResponseModel> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericDeleteHandle()
        {
            _genericRepository = new GenericRepository<TEntity>();
        }

        public async Task<ResponseModel> Handle(GenericDeleteRequest<TEntity> request, CancellationToken cancellationToken)
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
                return new ResponseModel { Message = "Id not matched - Deleting failed!" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message) };
            }
        }
    }
}
