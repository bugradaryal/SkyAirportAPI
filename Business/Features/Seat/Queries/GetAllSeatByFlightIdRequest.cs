﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Features.Seat.Queries
{
    public record GetAllSupportTicketRequest(int id) : IRequest<GetAllSupportTicketResponse>;
}
