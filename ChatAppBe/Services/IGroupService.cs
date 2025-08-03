using ChatAppBe.Data.Models.Response;

namespace ChatAppBe.Services
{
    public interface IGroupService
    {
        Task<int> CreateGroupAsync(string groupName);
        Task<bool> AddUserToGroupAsync(int groupId, int userId);
        Task<List<GroupResponse>> GetGroupMembersAsync(int userId);
       
    }
}
