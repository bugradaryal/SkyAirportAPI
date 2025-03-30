using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ExceptionHandler
{
    public class CustomException : Exception
    {
        public int ErrorCode { get; }

        public CustomException(string message, int errorCode = 400) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
