using ChatAppBe.Data.Models.Request;

namespace ChatAppBe.Services
{
    public interface IGroupMessageService
    {
        Task SendGroupMessageAsync(SendGroupMessageRequest request);
    }
}
