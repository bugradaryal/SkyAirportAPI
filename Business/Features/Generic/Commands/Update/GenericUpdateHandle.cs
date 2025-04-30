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

namespace Business.Features.Generic.Commands.Update
{
    public class GenericUpdateHandle<TEntity> : IRequestHandler<GenericUpdateRequest<TEntity>, CustomException> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericUpdateHandle()
        {
            _genericRepository = new GenericRepository<TEntity>();
        }

        public async Task<CustomException> Handle(GenericUpdateRequest<TEntity> request, CancellationToken cancellationToken)
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
                return new CustomException("Id not matched - Update failed!!", (int)HttpStatusCode.NotFound);
            }
            catch (Exception ex) 
            {
                return new CustomException(ex.Message, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message);
            }
        }
    }
}
