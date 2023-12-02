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

        [Required(ErrorMessage = "Почтовый адрес не указан")]
        [MaxLength(40)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string? Email { get; set; }

        public int EventId { get; set; }

        [Required]
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; } = new();

        public virtual List<ApplicationData>? ApplicationData { get; set; }
    }
}
