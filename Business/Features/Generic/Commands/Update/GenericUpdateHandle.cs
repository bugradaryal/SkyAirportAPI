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

namespace Business.Features.Generic.Commands.Update
{
    public class GenericUpdateHandle<TEntity> : IRequestHandler<GenericUpdateRequest<TEntity>, ResponseModel> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericUpdateHandle()
        {
            _genericRepository = new GenericRepository<TEntity>();
        }

        public async Task<ResponseModel> Handle(GenericUpdateRequest<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = request.ToEntity();
                var entityId = entity.GetType().GetProperty("id").GetValue(entity);
                if(entityId != null)
                {
                    if(await _genericRepository.Any((int)entityId))
                    {
                        await _genericRepository.Update(request.ToEntity());
                        return null;
                    }
                }
                return new ResponseModel { Message = "Id not matched - Update failed!!" };
            }
            catch (Exception ex) 
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message) };
            }
        }
    }
}
