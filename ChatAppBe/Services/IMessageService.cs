using ChatAppBe.Data.Models.Request;

namespace ChatAppBe.Services
{
    public interface IMessageService
    {
        bool SendMessage(SendMessageRequest request);
    }
}
