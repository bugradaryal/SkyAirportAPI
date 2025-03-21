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
        public string Status { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Uptaded_at { get; set; }


        public ICollection<Ticket> ticket { get; set; }
        public ICollection<AdminOperation> adminOperation { get; set; }
        public ICollection<LogEntry> logEntry { get; set; }
    }
}
