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

namespace Business.Features.Generic.Commands.Add
{
    public class GenericAddHandle<TEntity> : IRequestHandler<GenericAddRequest<TEntity>, ResponseModel> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericAddHandle()
        {
            _genericRepository = new GenericRepository<TEntity>();
        }

        public async Task<ResponseModel> Handle(GenericAddRequest<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                await _genericRepository.Add(request.ToEntity());
                return null;
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message) };
            }
        }
    }
}
