using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Account
{
    public class UpdateAccountDTO
    {
        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string Surname { get; set; }
        [RegularExpression(@"^(E|K|U)$")]
        public char Gender { get; set; }
        [Range(1, 120)]
        [Required]
        public int Age { get; set; }
        [Phone]
        [Required]
        [StringLength(16, MinimumLength = 10)]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression(@"^\+[0-9]{1,4}$", ErrorMessage = "Invalid country code format.")]
        [StringLength(5, MinimumLength = 2)]
        public string CountryCode { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 5)]
        public string UserName { get; set; }
    }
}
