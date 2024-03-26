using System.ComponentModel.DataAnnotations.Schema;

namespace Security_and_Auditing_Project_.NET_MVC.Models
{
    public class Notes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
