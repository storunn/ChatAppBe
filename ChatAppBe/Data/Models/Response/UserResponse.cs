namespace ChatAppBe.Data.Models.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int UserId { get; internal set; }
    }
}
