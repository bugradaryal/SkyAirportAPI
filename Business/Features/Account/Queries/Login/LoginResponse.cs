using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Entities;
using Utilitys.ResponseHandler;

namespace Business.Features.Account.Queries.Login
{
    public class LoginResponse
    {
        public User? user { get; set; }
        public ResponseModel? response { get; set; }
        public bool error { get; set; } = true;
    }
}
