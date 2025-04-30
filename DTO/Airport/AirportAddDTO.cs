using System.ComponentModel.DataAnnotations;

namespace DTO.Airport
{
    public class AirportAddDTO
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(512)]
        public string Location { get; set; }

        [Required]
        [Phone]
        [StringLength(16)]
        public string PhoneNumber { get; set; }

        [Url]
        [StringLength(96)]
        public string MailAdress { get; set; } = "Undefined";

        [StringLength(1024)]
        public string Description { get; set; } = "No Description";

        [StringLength(32)]
        public string Status { get; set; }
    }
}
