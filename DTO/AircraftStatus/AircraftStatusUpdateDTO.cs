using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.AircraftStatus
{
    public class AircraftStatusUpdateDTO
    {
        [Required]
        public int id { get; set; }
        [MaxLength(64)]
        public string Status { get; set; }
    }
}
