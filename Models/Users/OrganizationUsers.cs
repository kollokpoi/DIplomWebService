using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomService.Models.Users
{
    [Table("OrganizationUsers")]
    public class OrganizationUsers : User
    {
        [Required]
        public bool OrganizationLeader { get; set; } = false;


        [Required]
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; } = new();
    }
}
