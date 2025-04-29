using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Entities;

namespace Business.Features.Account.Queries.Login
{
    public class LoginResponse
    {
        public User user { get; set; }
        public CustomException exception { get; set; }
        public bool error { get; set; } = true;
    }
}
