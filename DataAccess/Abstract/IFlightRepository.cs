﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DataAccess.Abstract
{
    public interface IFlightRepository
    {
        Task<List<Flight>> GetAllByAirlineId(int id);
        Task<List<Flight>> GetAllByAircraftId(int id);
    }
}
