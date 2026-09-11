using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace Business.Features.Account.Queries.GetUserByEmail
{
    public class GetUserByEmailResponse
    {
        public User? user { get; set; }
    }
}
