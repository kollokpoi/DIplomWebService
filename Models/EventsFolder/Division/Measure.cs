using DiplomService.Models.EventsFolder.Division;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DiplomService.Models
{
    public class Measure
    {
        [Key]
        public int Id { get; set; }
        public byte[]? Icon { get; set; }
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Не указано место проведения")]
        public string Place { get; set; } = "";

        public string? Descrition { get; set; } = "";

        public TimeSpan Length { get; set; } = TimeSpan.Zero;

        [Required(ErrorMessage = "Пропущен пункт")]
        public bool SameForAll { get; set; } = true;

        [Required]
        public bool OneTime { get; set; } = false;
        [Required]
        public bool WeekDays { get; set; } = false;

        public int EventId { get; set; }

        [Required]
        [ForeignKey("EventId")]
        [JsonIgnore]
        public virtual Event? Event { get; set; } = new();
        public virtual List<MeasureDivisionsInfo> MeasureDivisionsInfos { get; set; } = new();
        [NotMapped]
        public string? MimeType { get { return GetImageMimeType(); } }
        private string GetImageMimeType()
        {
            if (Icon == null) return "application/octet-stream";
            if (Icon.Length < 4)
            {
                return "application/octet-stream"; // По умолчанию, если массив слишком короткий для определения
            }

            if (Icon[0] == 0xFF && Icon[1] == 0xD8 && Icon[2] == 0xFF)
            {
                return "image/jpeg";
            }
            else if (Icon[0] == 0x89 && Icon[1] == 0x50 && Icon[2] == 0x4E && Icon[3] == 0x47)
            {
                return "image/png";
            }
            // Добавьте другие проверки для других форматов, если это необходимо

            return "application/octet-stream"; // Если формат неизвестен, вернуть по умолчанию
        }
    }
}
