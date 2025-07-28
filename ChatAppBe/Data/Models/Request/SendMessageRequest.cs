namespace ChatAppBe.Data.Models.Request
{
    public class SendMessageRequest
    {
        public int? SenderUserId { get; set; }
        public string? SenderUsername { get; set; }
        public int? ReceiverUserId { get; set; }
        public string? ReceiverUsername { get; set; }
        public string Msg { get; set; }
    }
}
