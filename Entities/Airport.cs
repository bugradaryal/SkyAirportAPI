using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Airport
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string MailAdress { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public ICollection<Airline> airline { get; set; }
        public ICollection<Personal> personal { get; set; }
    }
}
