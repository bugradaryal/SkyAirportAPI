using DTO;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAccountServices
    {
        Task CreateAccount(CreateAccountDTO createAccountDTO);
        Task<User> LoginAccount(LoginAccountDTO loginAccountDTO);
        Task<List<string>> GetUserRoles(string userId);
        Task DeleteAccount(User user);
        Task UpdateAccount(User user);
        Task ChangePassword(User user, ChangePasswordDTO changePasswordDTO);
    }
}
