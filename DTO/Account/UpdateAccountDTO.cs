using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTO.Account
{
    public class UpdateAccountDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public char Gender { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string UserName { get; set; }
    }
}
