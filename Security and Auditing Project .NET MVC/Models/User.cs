﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Security_and_Auditing_Project_.NET_MVC.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender Gender { get; set; }
    }
}