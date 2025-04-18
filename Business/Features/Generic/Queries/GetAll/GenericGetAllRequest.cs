using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using MediatR;

namespace Business.Features.Generic.Queries.GetAll
{
    public record GenericGetAllRequest<TEntity> : IRequest<GenericGetAllResponse<TEntity>>;
}
