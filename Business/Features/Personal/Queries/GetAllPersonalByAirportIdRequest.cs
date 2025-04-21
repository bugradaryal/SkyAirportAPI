using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using Entities;
using MediatR;

namespace Business.Features.Personal.Queries
{
    public record GetAllPersonalByAirportIdRequest(int id) : IRequest<GetAllPersonalByAirportIdResponse>;
}
