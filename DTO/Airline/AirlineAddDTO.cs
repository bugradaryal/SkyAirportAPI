﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Airline
{
    public class AirlineAddDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebAdress { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public int airport_id { get; set; }
    }
}
