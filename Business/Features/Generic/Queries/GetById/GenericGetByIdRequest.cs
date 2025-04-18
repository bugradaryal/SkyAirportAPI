using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.Generic.Queries.GetById
{
    public record GenericGetByIdRequest<TEntity>(int objectId) :IRequest<GenericGetByIdResponse<TEntity>>;
}
