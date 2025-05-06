using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Aircraft
{
    public class AircraftUpdateDTO
    {
        public int id { get; set; }
        public string Model { get; set; }
        public decimal Fuel_Capacity { get; set; }
        public decimal Max_Altitude { get; set; }
        public int Engine_Power { get; set; }
        public decimal Carry_Capacity { get; set; }
        public DateTimeOffset Last_Maintenance_Date { get; set; }
        public int aircraftStatus_id { get; set; }
    }
}
