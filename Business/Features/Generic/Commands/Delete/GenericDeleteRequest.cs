using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using MediatR;
using Utilitys.ResponseHandler;

namespace Business.Features.Generic.Commands.Delete
{
    public record GenericDeleteRequest<TEntity>(int objectId) : IRequest<ResponseModel>;
}
