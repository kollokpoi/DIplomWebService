using DiplomService.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomService.Models
{
    public class EventApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime TimeOfSend { get; set; }
        public string Institution { get; set; } = "";
        public bool? Accepted { get; set; } = null;
        [Required(ErrorMessage = "Почтовый адрес не указан")]
        [MaxLength(40)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string? Email { get; set; }

        public int EventId { get; set; }
        public string ApplicationSenderId { get; set; } = "";

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; } = new();
        [ForeignKey("ApplicationSenderId")]
        public virtual WebUser ApplicationSender { get; set; } = new();
        public virtual List<ApplicationData> ApplicationData { get; set; } = new();
    }
}
