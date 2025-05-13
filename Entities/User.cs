using Entities.Moderation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public char Gender { get; set; }
        public int Age { get; set; }
        public bool IsSuspended { get; set; }
        public DateTimeOffset Created_at { get; set; }
        public DateTimeOffset Uptaded_at { get; set; }
        public string CountryCode { get; set; }

        public ICollection<OwnedTicket> ownedTickets { get; set; }
        public ICollection<LogEntry> logEntry { get; set; }
    }
}
