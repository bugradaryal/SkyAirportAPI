﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DataAccess.Abstract
{
    public interface IOwnedTicketRepository
    {
        Task<decimal> GetTicketWeightById(int id);
    }
}
