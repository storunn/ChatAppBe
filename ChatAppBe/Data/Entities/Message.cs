namespace ChatAppBe.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string Msg { get; set; }
    }
}
