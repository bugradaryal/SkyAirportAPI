﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Ticket
    {
        public int id { get; set; }
        public decimal Price { get; set; }
        public int seat_id { get; set; }

        public OwnedTicket ownedTicket { get; set; }
        public Seat seat { get; set; }
    }
}
