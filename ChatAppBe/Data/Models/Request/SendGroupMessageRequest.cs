namespace ChatAppBe.Data.Models.Request
{
    public class SendGroupMessageRequest
    {
        public int GroupId { get; set; }
        public int SenderUserId { get; set; }
        public string Msg { get; set; } = string.Empty;
    }
}
