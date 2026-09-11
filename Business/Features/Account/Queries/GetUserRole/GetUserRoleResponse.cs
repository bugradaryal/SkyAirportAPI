using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace Business.Features.Account.Queries.GetUserRole
{
    public class GetUserRoleResponse
    {
        public List<string>? UserRoles { get; set; }
    }
}
