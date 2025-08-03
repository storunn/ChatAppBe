namespace ChatAppBe.Data.Entities
{
    public class GroupMessage
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public int SenderUserId { get; set; }
        public User? SenderUser { get; set; }

        public string? Msg { get; set; }
        public DateTime SentAt { get; set; }


    }
}
