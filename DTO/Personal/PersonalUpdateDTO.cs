using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Personal
{
    public class PersonalUpdateDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset Start_Date { get; set; }

        public int airport_id { get; set; }
    }
}
