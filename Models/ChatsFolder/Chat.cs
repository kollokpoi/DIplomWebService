using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DiplomService.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        public string FirstUserId { get; set; } = "";

        [Required]
        [ForeignKey("FirstUserId")]
        [JsonIgnore]
        public virtual User? FirstUser { get; set; }

        public string SecondUserId { get; set; } = "";

        [Required]
        [ForeignKey("SecondUserId")]
        [JsonIgnore]
        public virtual User? SecondUser { get; set; }

        private int typeId = 2;

        [Required]
        public int TypeId
        {
            private get { return typeId; }
            set
            {
                typeId = value;
                ChatType = value == 1 ? ChatTypes.WebChat : ChatTypes.ApplicationChat;
            }
        }

        public virtual List<Message>? Messages { get; set; }

        [NotMapped]
        public ChatTypes ChatType { get; set; }

        public enum ChatTypes
        {
            WebChat = 1,
            ApplicationChat = 2
        }
    }
}
