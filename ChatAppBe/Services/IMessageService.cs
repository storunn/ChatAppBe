using ChatAppBe.Data.Models.Request;
using ChatAppBe.Data.Models.Response;

namespace ChatAppBe.Services
{
    public interface IMessageService
    {
        Task<bool> SendMessageAsync(SendMessageRequest request);
        List<MessageResponse> GetMessagesByUserId(int userId);
        List<MessageResponse> GetMessagesByUsername(string username);
    }
}
