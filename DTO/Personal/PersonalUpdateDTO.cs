using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Personal
{
    public class PersonalUpdateDTO
    {
        [Required]
        public int id { get; set; }

        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(64)]
        public string Surname { get; set; }

        [StringLength(256)]
        public string Role { get; set; }

        [Range(18, 120)]  // Age için 18 ile 120 arasında bir değer
        public int Age { get; set; }

        [Column(TypeName = "char(1)")]
        [RegularExpression(@"^(E|K|U)$")]
        public char Gender { get; set; }

        [Phone]
        [StringLength(16)]  // PhoneNumber için 16 karakter
        public string PhoneNumber { get; set; }

        public DateTimeOffset Start_Date { get; set; }

        public int airport_id { get; set; }
    }
}
