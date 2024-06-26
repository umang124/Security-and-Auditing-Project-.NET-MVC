﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Security_and_Auditing_Project_.NET_MVC.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
    }
}
