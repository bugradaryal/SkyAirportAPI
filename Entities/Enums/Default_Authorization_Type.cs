using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enums
{
    public class Default_Authorization_Type
    {
        public enum Roles
        {
            Administrator,
            User
        }
        public const string default_username = "user";
        public const string default_email = "user@secureapi.com";
        public const string default_password = "Pa$$w0rd.";
        public const Roles default_role = Roles.User;
    }
}
