using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomService.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Upload)]
        public byte[]? PriviewImage { get; set; } = null;

        [MaxLength(40)]
        [Display(Name = "Название*")]
        public string Name { get; set; } = "";

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Дата начала*")]
        public DateTime DateOfStart { get; set; }

        [Display(Name = "Дата окончания")]
        public DateTime? DateOfEnd { get; set; }

        [Display(Name = "Является ли событие публичным*")]
        public bool PublicEvent { get; set; } = true;

        [Required]
        public bool ReadyToShow { get; set; } = false;

        [Required]
        public bool DivisionsExist { get; set; } = true;


        public virtual List<Organization> Organizations { get; set; } = new();
        public virtual List<Measure> Measures { get; set; } = new();
        public virtual List<Division> Divisions { get; set; } = new();
        public virtual List<News> News { get; set; } = new();
        public virtual List<EventApplication>? EventApplications { get; set; }

        [NotMapped]
        public string? MimeType { get { return GetImageMimeType(); } }
        private string GetImageMimeType()
        {
            if (PriviewImage == null) return "application/octet-stream";
            if (PriviewImage.Length < 4)
            {
                return "application/octet-stream"; // По умолчанию, если массив слишком короткий для определения
            }

            if (PriviewImage[0] == 0xFF && PriviewImage[1] == 0xD8 && PriviewImage[2] == 0xFF)
            {
                return "image/jpeg";
            }
            else if (PriviewImage[0] == 0x89 && PriviewImage[1] == 0x50 && PriviewImage[2] == 0x4E && PriviewImage[3] == 0x47)
            {
                return "image/png";
            }
            // Добавьте другие проверки для других форматов, если это необходимо

            return "application/octet-stream"; // Если формат неизвестен, вернуть по умолчанию
        }

        [NotMapped]
        public int EventMembers
        {
            get
            {
                int result = 0;

                foreach (var item in Divisions)
                {
                    result += item.DivisionMembers.Count;
                }

                return result;
            }
        }
    }
}
