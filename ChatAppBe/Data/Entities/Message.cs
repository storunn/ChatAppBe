using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppBe.Data.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderUserId { get; set; }
        public User SenderUser { get; set; }

        [Required]
        public int ReceiverUserId { get; set; }
        public User ReceiverUser { get; set; }

        [Required]
        public string Msg { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}