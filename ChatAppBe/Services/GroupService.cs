using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Entities;
using ChatAppBe.Data.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBe.Services
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;

        public GroupService(AppDbContext context)
        {
            _context = context;
        }

        // 1. Grup oluştur
        public async Task<int> CreateGroupAsync(string groupName)
        {
            var group = new Group
            {
                Name = groupName
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group.Id;
        }

        // 2. Gruba kullanıcı ekle
        public async Task<bool> AddUserToGroupAsync(int groupId, int userId)
        {
            var alreadyExists = await _context.GroupMembers
                .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

            if (alreadyExists)
                return false;

            _context.GroupMembers.Add(new GroupMember
            {
                GroupId = groupId,
                UserId = userId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        // 3. Kullanıcının gruplarını getir
        public async Task<List<GroupResponse>> GetGroupMembersAsync(int userId)
        {
            return await _context.GroupMembers
                .Where(gm => gm.UserId == userId)
                .Include(gm => gm.Group)
                .Select(gm => new GroupResponse
                {
                    GroupId = gm.GroupId,
                    GroupName = gm.Group.Name
                })
                .ToListAsync();
        }

      
    }
}
