﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Aircraft
    {
        public int id { get; set; }
        public string Model { get; set; }
        public DateTimeOffset? Last_Maintenance_Date { get; set; }
        public decimal Fuel_Capacity { get; set; }
        public decimal Max_Altitude { get; set; }
        public int Engine_Power { get; set; }
        public decimal Carry_Capacity { get; set; }
        public int aircraftStatus_id { get; set; }
        public decimal Current_Capacity { get; set; }

        public AircraftStatus aircraftStatus { get; set; }
        public ICollection<Flight_Aircraft> flight_Aircraft { get; set; }
        public ICollection<Crew_Aircraft> crew_Aircraft { get; set; }
    }
}
