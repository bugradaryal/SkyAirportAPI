using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMailServices
    {
        Task ConfirmEmail(string userid, string token);
        Task SendingEmail(string email, string url);
    }
}
