﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Account
{
    public class ValidateTokenDTO
    {
        public User user { get; set; }
        public List<string> roles { get; set; }
        public bool IsTokenValid { get; set; }
    }
}
