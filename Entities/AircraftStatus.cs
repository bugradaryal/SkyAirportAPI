using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class AircraftStatus
    {
        public int id { get; set; }
        public string Status { get; set; }


        public ICollection<Aircraft> aircraft { get; set; }
    }
}
