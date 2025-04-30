using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitys.ExceptionHandler
{
    public class CustomException : Exception
    {
        public int ErrorCode { get; }
        public string? InnerMessage { get; }

        public CustomException(string message, int errorCode = 400, string? innerMessage = null) : base(message)
        {
            ErrorCode = errorCode;
            InnerMessage = innerMessage;
        }
    }
}
