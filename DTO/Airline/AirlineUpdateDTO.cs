using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Airline
{
    public class AirlineUpdateDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebAdress { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public int airport_id { get; set; }
    }
}
