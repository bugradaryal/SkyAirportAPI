using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Seat
{
    public class SeatAddDTO
    {
        [Required]
        public int Seat_number { get; set; }

        [Required]
        [StringLength(32)]  
        public string Seat_Class { get; set; }

        [Required]
        [StringLength(64)]  
        public string Location { get; set; }

        [Required]
        public int flight_id { get; set; }
    }
}
