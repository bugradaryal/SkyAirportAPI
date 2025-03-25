using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Airline
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebAdress { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }


        public int airport_id { get; set; }
        public ICollection<Flight> flight { get; set; }
        public Airport airport { get; set; }
    }
}
