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
        [Required]
        public int id { get; set; }
        [MaxLength(64)]
        public string Model { get; set; }

        [Range(0, 999999.9)]
        public decimal Fuel_Capacity { get; set; }

        [Range(0, 999999.9)]
        public decimal Max_Altitude { get; set; }

        [Range(0, int.MaxValue)]
        public int Engine_Power { get; set; }

        [Range(0, 999999.9)]
        public decimal Carry_Capacity { get; set; }
        public DateTimeOffset Last_Maintenance_Date { get; set; }
        public int aircraftStatus_id { get; set; }
    }
}
