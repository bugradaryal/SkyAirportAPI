using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ResponseHandler;

namespace Business.Abstract
{
    public interface IMailServices
    {
        Task<ResponseModel> ConfirmEmail(string userid, string token);
        Task<ResponseModel> SendingEmail(string email, string url);
    }
}
