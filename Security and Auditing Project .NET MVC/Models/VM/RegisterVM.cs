using System.ComponentModel.DataAnnotations;

namespace Security_and_Auditing_Project_.NET_MVC.Models.VM
{
    public class RegisterVM
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
