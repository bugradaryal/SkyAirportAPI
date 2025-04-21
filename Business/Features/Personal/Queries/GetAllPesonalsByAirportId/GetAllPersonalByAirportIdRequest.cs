using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using Business.Features.Personal.Queries.GetAllPesonalsByAirportId;
using Entities;
using MediatR;

namespace Business.Features.Personal.Commands.GetAllPesonalsByAirportId
{
    public record GetAllPersonalByAirportIdRequest(int id) : IRequest<GetAllPersonalByAirportIdResponse>;
}
