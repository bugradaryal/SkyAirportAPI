using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTO.Personal
{
    public class PersonalAddDTO
    {
        [Required]
        [StringLength(64)]  
        public string Name { get; set; }

        [Required]
        [StringLength(64)]  
        public string Surname { get; set; }

        [Required]
        [StringLength(256)]  
        public string Role { get; set; }

        [Required]
        [Range(18, 120)]  // Age için 18 ile 120 arasında bir değer
        public int Age { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        [RegularExpression(@"^(E|K|U)$")]
        public char Gender { get; set; }

        [Required]
        [Phone]
        [StringLength(16)]  // PhoneNumber için 16 karakter
        public string PhoneNumber { get; set; }
        public DateTimeOffset Start_Date { get; set; }

        [Required]
        public int airport_id { get; set; }
    }
}
