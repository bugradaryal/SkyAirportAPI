using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Airport
{
    public class AirportUpdateDTO
    {
        [Required]
        public int id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(512)]
        public string Location { get; set; }

        [Phone]
        [StringLength(16)]
        public string PhoneNumber { get; set; }

        [StringLength(96)]
        [Url]
        public string MailAdress { get; set; } = "Undefined";

        [StringLength(1024)]
        public string Description { get; set; } = "No Description";

        [Required]
        [StringLength(32)]
        public string Status { get; set; }
    }
}
