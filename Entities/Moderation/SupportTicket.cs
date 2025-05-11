using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Moderation
{
    public class SupportTicket
    {
        public int id { get; set; }
        public string UserId { get; set; }  // Kullanıcı kimliği
        public string Message { get; set; } // Talep mesajı
        public string Status { get; set; }  // Durum (Yeni, Çözüldü vb.)
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ResolvedAt { get; set; }
    }
}
