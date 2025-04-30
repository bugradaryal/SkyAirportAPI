using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Airline
{
    public class AirlineAddDTO
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [Required]
        [Url]
        [MaxLength(128)]
        public string WebAdress { get; set; }

        [Phone]
        [MaxLength(16)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(60)]
        public string Country { get; set; }

        [Required]
        public int airport_id { get; set; }
    }
}
