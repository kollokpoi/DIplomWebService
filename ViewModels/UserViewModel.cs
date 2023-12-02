using DiplomService.Services.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DiplomService.ViewModels
{
    public class UserViewModel
    {

        public string Id { get; set; } = "";

        [Required(ErrorMessage = "Не указано имя")]
        [Display(Name = "Имя")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        [Display(Name = "Фамилия")]
        public string? SecondName { get; set; }

        [Required(ErrorMessage = "Не указано отчество")]
        [Display(Name = "Отчество")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Не указана почта")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Адрес электронной почты")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string? Email { get; set; } = "";

        [Display(Name = "Номер телефона")]
        [RussianPhoneNumber(ErrorMessage = "Введите действительный номер телефона")]
        public string? PhoneNumber { get; set; }
    }
}
