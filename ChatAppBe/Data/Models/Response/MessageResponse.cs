namespace ChatAppBe.Data.Models.Response
{
    public class MessageResponse
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public string SenderUsername { get; set; }
        public int ReceiverUserId { get; set; }
        public string ReceiverUsername { get; set; }
        public string Msg { get; set; }
    }
}
