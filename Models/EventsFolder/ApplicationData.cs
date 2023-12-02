using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomService.Models
{
    public class ApplicationData
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        public string? SecondName { get; set; }

        [Required(ErrorMessage = "Не указано отчество")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Не указана дата рождения")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Не указан номер телефона")]
        public string PhoneNumber { get; set; } = "Номер телефона не указан";

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string? Email { get; set; } = "Почта не указана";

        public string? Institution { get; set; } = "";

        [Range(1, 5, ErrorMessage = "Недопустимый курс")]
        public int? Course { get; set; }


        public int ApplicationId { get; set; }

        [Required]
        [ForeignKey("ApplicationId")]
        public virtual EventApplication Application { get; set; } = new();
    }
}
