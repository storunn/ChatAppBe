using ChatAppBe.Data.Models.Response;

namespace ChatAppBe.Services
{
    public interface IGroupService
    {
        Task<int> CreateGroupAsync(string groupName, int creatorUserId);
        Task<bool> AddUserToGroupAsync(int groupId, string username);
        Task<List<GroupResponse>> GetUserGroupsAsync(int userId);
        Task<List<UserResponse>> GetMembersOfGroupAsync(int groupId);
        Task<bool> LeaveGroupAsync(int groupId, int userId);
        Task<bool> RemoveUserFromGroupAsync(int groupId, int userId);

    }
}
