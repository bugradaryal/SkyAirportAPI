using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AircraftDTO
    {
        public int id { get; set; }
        public string Model { get; set; }
        public DateTime Last_Maintenance_Date { get; set; }
        public double Fuel_Capacity { get; set; }
        public double Max_Altitude { get; set; }
        public int Engine_Power { get; set; }
        public double Carry_Capacity { get; set; }
    }
}
