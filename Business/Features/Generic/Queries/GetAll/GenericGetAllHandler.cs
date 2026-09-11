using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        public GenericGetAllHandler(IGenericRepository<TEntity> genericRepository)
        {
            _repository = genericRepository;
        }

        public async Task<GenericGetAllResponse<TEntity>> Handle(GenericGetAllRequest<TEntity> request, CancellationToken cancellationToken)
        {
            return new GenericGetAllResponse<TEntity> { entity = await _repository.GetAll() };
        }
    }
}
