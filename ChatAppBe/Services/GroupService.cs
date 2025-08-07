using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Entities;
using ChatAppBe.Data.Models.Response;
using Microsoft.AspNetCore.Mvc;
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

        
        public async Task<int> CreateGroupAsync(string name, int creatorUserId)
        {
            // 1. Grup kaydı ekle
            var group = new Group { Name = name };
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            // 2. Grubu oluşturan kullanıcıyı gruba otomatik ekle
            var groupMember = new GroupMember { GroupId = group.Id, UserId = creatorUserId };
            _context.GroupMembers.Add(groupMember);
            await _context.SaveChangesAsync();

            return group.Id;
        }



        // 2. Gruba kullanıcı ekle
        public async Task<bool> AddUserToGroupAsync(int groupId, string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return false; // Kullanıcı bulunamadı

            var alreadyExists = await _context.GroupMembers
                .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == user.Id);

            if (alreadyExists)
                return false;

            _context.GroupMembers.Add(new GroupMember
            {
                GroupId = groupId,
                UserId = user.Id
            });

            await _context.SaveChangesAsync();
            return true;
        }

        // 3. Kullanıcının gruplarını getir
        
        public async Task<List<GroupResponse>> GetUserGroupsAsync(int userId)
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

       
        public async Task<bool> LeaveGroupAsync(int groupId, int userId)
        {
            var membership = await _context.GroupMembers
                .FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

            if (membership == null)
                return false;

            _context.GroupMembers.Remove(membership);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveUserFromGroupAsync(int groupId, int userId)
        {
            var membership = await _context.GroupMembers
                .FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

            if (membership == null)
                return false;

            _context.GroupMembers.Remove(membership);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserResponse>> GetMembersOfGroupAsync(int groupId)
        {
            // GroupMember entity'sinde UserId ve GroupId alanları var, User tablosu ile ilişkili
            var members = await _context.GroupMembers
                .Where(gm => gm.GroupId == groupId)
                .Select(gm => new UserResponse
                {
                    UserId = gm.UserId,
                    Username = gm.User.Username 
                })
                .ToListAsync();

            return members;
        }

    }

}
