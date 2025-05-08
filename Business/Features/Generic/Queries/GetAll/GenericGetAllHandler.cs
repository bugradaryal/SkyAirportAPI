using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Account.Queries.GetUserRole;
using Business.Features.Account.Queries.Login;
using DataAccess.Abstract;
using DataAccess.Concrete.Generic;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Generic.Queries.GetAll
{
    public class GenericGetAllHandler<TEntity> : IRequestHandler<GenericGetAllRequest<TEntity>, GenericGetAllResponse<TEntity>> where TEntity : class
    {
        private IGenericRepository<TEntity> _repository;
        public GenericGetAllHandler()
        {
            _repository = new GenericRepository<TEntity>();
        }

        public async Task<GenericGetAllResponse<TEntity>> Handle(GenericGetAllRequest<TEntity> request, CancellationToken cancellationToken)
        {
            try
            {
                return new GenericGetAllResponse<TEntity> { data = await _repository.GetAll(), error = false };
            }
            catch (Exception ex)
            {
                return new GenericGetAllResponse<TEntity> { response = { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest, ex.InnerException?.Message) } };
            }
        }
    }
}
