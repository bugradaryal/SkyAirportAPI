using System.ComponentModel.DataAnnotations;

namespace DTO.Airport
{
    public class AirportAddDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string MailAdress { get; set; } = "Undefined";
        public string Description { get; set; } = "No Description";
        public string Status { get; set; }
    }
}
