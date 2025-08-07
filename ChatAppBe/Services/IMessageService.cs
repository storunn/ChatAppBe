using ChatAppBe.Data.Models.Request;
using ChatAppBe.Data.Models.Response;

namespace ChatAppBe.Services
{
    public interface IMessageService
    {
        Task<bool> SendMessageAsync(SendMessageRequest request);
        List<MessageResponse> GetMessagesByUserId(int userId);
        List<MessageResponse> GetMessagesByUsername(string username);
        Task<List<PrivateMessageResponse>> GetPrivateMessagesAsync(string username1, string username2);
      


    }
}
