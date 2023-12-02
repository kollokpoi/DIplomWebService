using DiplomService.Models.Users;
using Microsoft.Build.Framework;

namespace DiplomService.Models
{
    public class DivisionUsers
    {
        public int Id { get; set; }
        [Required]
        public virtual Division Division { get; set; } = new();
        [Required]
        public virtual MobileUser User { get; set; } = new();
        [Required]
        public bool DivisionDirector { get; set; } = false;
    }
}
