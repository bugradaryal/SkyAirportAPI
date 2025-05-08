using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using MediatR;
using Utilitys.ResponseHandler;

namespace Business.Features.Generic.Commands.Update
{
    public record GenericUpdateRequest<TEntity>(TEntity Entity) : IRequest<ResponseModel>, IConvertToEntity<TEntity>
    {
        public TEntity ToEntity() => Entity;
    }
}
