using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Generic.Queries.GetAll;
using DataAccess.Abstract;
using DataAccess.Concrete.Generic;
using MediatR;

namespace Business.Features.Generic.Queries.GetById
{
    public class GenericGetByIdHandler<TEntity> : IRequestHandler<GenericGetByIdRequest<TEntity>, GenericGetByIdResponse<TEntity>> where TEntity : class
    {
        private IGenericRepository<TEntity> _repository;
        public GenericGetByIdHandler() 
        {
            _repository = new GenericRepository<TEntity>();
        }

        public async Task<GenericGetByIdResponse<TEntity>> Handle(GenericGetByIdRequest<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                return new GenericGetByIdResponse<TEntity> { entity = await _repository.GetValue(request.objectId), error = false };
            }
            catch (Exception ex)
            {
                return new GenericGetByIdResponse<TEntity> { response = { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message) } };
            }
        }
    }
}
