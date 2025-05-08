using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using MediatR;
using Utilitys.ResponseHandler;

namespace Business.Features.Generic.Commands.Add
{
    public record GenericAddRequest<TEntity>(TEntity Entity) : IRequest<ResponseModel>, IConvertToEntity<TEntity>
    {
        public TEntity ToEntity() => Entity;
    }
}
