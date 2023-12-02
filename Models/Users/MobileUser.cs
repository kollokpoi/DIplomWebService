using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomService.Models.Users
{
    [Table("MobileUsers")]
    public class MobileUser : User
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public virtual List<DivisionUsers> UserDivisions { get; set; } = new();
    }
}
