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

namespace Business.Features.Generic.Commands.Add
{
    public class GenericAddHandle<TEntity> : IRequestHandler<GenericAddRequest<TEntity>, CustomException> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericAddHandle()
        {
            _genericRepository = new GenericRepository<TEntity>();
        }

        public async Task<CustomException> Handle(GenericAddRequest<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                await _genericRepository.Add(request.ToEntity());
                return null;
            }
            catch (Exception ex)
            {
                return new CustomException(ex.Message, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message);
            }
        }
    }
}
