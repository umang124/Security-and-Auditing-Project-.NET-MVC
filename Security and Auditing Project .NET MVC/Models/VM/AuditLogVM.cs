using System.ComponentModel.DataAnnotations.Schema;

namespace Security_and_Auditing_Project_.NET_MVC.Models.VM
{
    public class AuditLogVM
    {
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
    }
}
