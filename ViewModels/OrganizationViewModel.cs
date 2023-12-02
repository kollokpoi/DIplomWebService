using DiplomService.Models;
using System.ComponentModel.DataAnnotations;

namespace DiplomService.ViewModels
{
    public class OrganizationViewModel
    {
        public Organization Organization { get; set; } = new();

        [Required]
        private IFormFile? imageFile = null;

        [Required(ErrorMessage = "Добавьте изображение")]

        public IFormFile? ImageFile
        {
            get { return imageFile; }
            set
            {
                imageFile = value;
                if (value != null)
                {
                    using var memoryStream = new MemoryStream();
                    value.CopyTo(memoryStream);
                    Organization.Preview = memoryStream.ToArray();
                }
            }
        }
    }
}
