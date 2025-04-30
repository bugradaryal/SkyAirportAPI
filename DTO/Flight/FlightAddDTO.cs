using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DTO.Flight
{
    public class FlightAddDTO
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(1024)]
        public string Description { get; set; }

        [Required]
        public DateTimeOffset Arrival_Date { get; set; }

        [Required]
        public DateTimeOffset Deperture_Date { get; set; }

        [Required]
        [StringLength(38)]
        public string Status { get; set; }
        [Required]
        public int airline_id { get; set; }
        [Required]
        public int aircraft_id { get; set; }
    }
}