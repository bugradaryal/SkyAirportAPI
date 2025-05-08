using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;

namespace Utilitys.ResponseHandler
{
    public class ResponseModel
    {
        public string? Message { get; set; }
        public CustomException? Exception { get; set; }
    }
}
