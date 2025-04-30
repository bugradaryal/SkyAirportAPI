using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Crew
{
    public class CrewUpdateDTO
    {
        [Required]
        public int id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string Surname { get; set; }

        [MaxLength(256)]
        public string Role { get; set; }

        [Range(18, 120)]
        public int Age { get; set; }

        [Column(TypeName = "char(1)")]
        [RegularExpression(@"^(E|K|U)$")]
        public char Gender { get; set; }
        public int aircraft_id { get; set; }
    }
}
