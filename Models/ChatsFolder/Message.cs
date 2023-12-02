using System.ComponentModel.DataAnnotations;

namespace DiplomService.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual User Sender { get; set; } = new User();

        [Required]
        public DateTime DateOfSend { get; set; } = DateTime.Now;

        [Required]
        public string Content { get; set; } = "";

    }
}
