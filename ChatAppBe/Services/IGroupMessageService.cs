using ChatAppBe.Data.Models.Request;
using ChatAppBe.Data.Models.Response;

namespace ChatAppBe.Services
{
    public interface IGroupMessageService
    {
        Task<List<GroupMessageResponse>> GetGroupMessagesAsync(int groupId);
        Task SendGroupMessageAsync(SendGroupMessageRequest request);
     
    }
}
