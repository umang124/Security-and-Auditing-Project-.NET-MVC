using System.ComponentModel.DataAnnotations;

namespace Security_and_Auditing_Project_.NET_MVC.Models.VM
{
    public class LoginVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
