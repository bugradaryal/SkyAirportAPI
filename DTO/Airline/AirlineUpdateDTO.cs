using System.ComponentModel.DataAnnotations;

namespace DTO.Airline
{
    public class AirlineUpdateDTO
    {
        [Required]
        public int id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [Url]
        [MaxLength(128)]
        public string WebAdress { get; set; }

        [Phone]
        [MaxLength(16)]
        public string PhoneNumber { get; set; }

        [MaxLength(60)]
        public string Country { get; set; }

        public int airport_id { get; set; }
    }
}
