using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Personal
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Start_Date { get; set; }


        public int airport_id { get; set; }
        public Airport airport { get; set; }
    }
}
