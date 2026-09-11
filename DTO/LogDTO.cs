using Entities.Enums;

namespace DTO
{
    public class LogDTO
    {
        public string Message { get; set; }
        public Action_Type Action_type { get; set; } = Action_Type.SystemError;
        public string? Target_table { get; set; }
        public List<string>? AdditionalData { get; set; } = null;
        public string? user_id { get; set; } = null;
        public int? loglevel_id { get; set; }
    }
}
