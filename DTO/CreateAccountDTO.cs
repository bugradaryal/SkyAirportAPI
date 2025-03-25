using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CreateAccountDTO
    {
        [Required]
        [StringLength(2, MinimumLength = 64)]
        public string Name { get; set; }
        [Required]
        [StringLength(2, MinimumLength = 64)]
        public string Surname { get; set; }
        [RegularExpression(@"^(E|K|U)$")]
        public char Gender { get; set; }
        [Range(1,120)]
        [Required]
        public int Age { get; set; }
        [EmailAddress]
        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string Email { get; set; }
        [Phone]
        [Required]
        [StringLength(16, MinimumLength = 10)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 5)]
        public string UserName { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 6)]
        [PasswordPropertyText(true)]
        public string Password { get; set; }
    }
}
