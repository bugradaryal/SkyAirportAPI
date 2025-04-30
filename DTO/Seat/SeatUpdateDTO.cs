using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Seat
{
    public class SeatUpdateDTO
    {
        [Required]
        public int id { get; set; }
        public int Seat_number { get; set; }

        [StringLength(32)]
        public string Seat_Class { get; set; }

        [StringLength(64)]
        public string Location { get; set; }

        public int flight_id { get; set; }
    }
}
