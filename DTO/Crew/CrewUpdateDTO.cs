using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Crew
{
    public class CrewUpdateDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }
        public int aircraft_id { get; set; }
    }
}
