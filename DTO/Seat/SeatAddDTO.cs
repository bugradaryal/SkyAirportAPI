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
        public int Seat_number { get; set; }
        public string Seat_Class { get; set; }
        public string Location { get; set; }
        public int flight_id { get; set; }
    }
}
