﻿using System;
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
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
