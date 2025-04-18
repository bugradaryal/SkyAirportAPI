using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using DataAccess.Abstract;
using DataAccess.Concrete.Generic;
using MediatR;

namespace Business.Features.Generic.Commands.Delete
{
    public class GenericDeleteHandle<TEntity> : IRequestHandler<GenericDeleteRequest, CustomException> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericDeleteHandle()
        {
            _genericRepository = new GenericRepository<TEntity>();
        }

        public async Task<CustomException> Handle(GenericDeleteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _genericRepository.Delete(request.objectId);
                return null;
            }
            catch (Exception ex)
            {
                return new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
