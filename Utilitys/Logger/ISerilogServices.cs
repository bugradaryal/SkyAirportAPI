using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace Utilitys.Logger
{
    public interface ISerilogServices
    {
        void Info(LogDTO logDTO);
        void Warn(LogDTO logDTO);
        void Error(LogDTO logDTO, Exception ex = null);
        public void Fatal(LogDTO logDTO, Exception ex = null);
    }
}
