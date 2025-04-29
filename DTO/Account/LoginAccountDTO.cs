using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Account
{
    public class LoginAccountDTO
    {
        [EmailAddress]
        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string Email { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 6)]
        [PasswordPropertyText(true)]
        public string Password { get; set; }
    }
}
