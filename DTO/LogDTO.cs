using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Enums;

namespace DTO
{
    public class LogDTO
    {
        public string Message { get; set; }
        public Action_Type Action_type { get; set; } = Action_Type.SystemError;
        public string? Target_table { get; set; }
        public int loglevel_id { get; set; } = 3;
        public List<string>? AdditionalData { get; set; } = null;
        public string? user_id { get; set; } = null;
    }
}
