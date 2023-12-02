using Microsoft.Build.Framework;

namespace DiplomService.Models
{
    public class Section
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        public byte[]? Image { get; set; }

        [Required]
        public virtual News News { get; set; } = new();
    }
}
