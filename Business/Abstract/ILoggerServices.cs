using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Utilitys.ExceptionHandler;

namespace Business.Abstract
{
    public interface ILoggerServices
    {
        Task Logger(LogDTO logdto, CustomException? exception = null);
    }
}
