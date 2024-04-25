using DiplomService.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiplomService.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateOfSend { get; set; } = DateTime.Now;

        [Required]
        public string Content { get; set; } = "";

        public string SenderId { get; set; } = string.Empty;

        [Required]
        [JsonIgnore]
        public virtual MobileUser Sender { get; set; } = new();
    }
}
