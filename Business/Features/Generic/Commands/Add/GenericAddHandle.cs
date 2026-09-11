using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using DataAccess.Concrete.Generic;
using MediatR;


namespace Business.Features.Generic.Commands.Add
{
    public class GenericAddHandle<TEntity> : IRequestHandler<GenericAddRequest<TEntity>> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericAddHandle(IGenericRepository<TEntity> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task Handle(GenericAddRequest<TEntity> request, CancellationToken cancellationToken)
        {
            await _genericRepository.Add(request.ToEntity());
        }
    }
}
