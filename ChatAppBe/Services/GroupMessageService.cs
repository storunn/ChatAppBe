using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Entities;
using ChatAppBe.Data.Models.Request;
using ChatAppBe.Data.Models.Response;
using ChatAppBe.Handlers;
using ChatAppBe.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBe.Services
{
    public class GroupMessageService : IGroupMessageService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public GroupMessageService(AppDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SendGroupMessageAsync(SendGroupMessageRequest request)
        {
            var message = new GroupMessage
            {
                GroupId = request.GroupId,
                SenderUserId = request.SenderUserId,
                Msg = request.Msg,
                SentAt = DateTime.Now
            };

            _context.GroupMessages.Add(message);
            await _context.SaveChangesAsync();

            // Gönderen kullanıcı adı
            var senderUser = _context.Users.FirstOrDefault(u => u.Id == request.SenderUserId);
            var senderUsername = senderUser?.Username ?? request.SenderUserId.ToString();

            // Grubun tüm üyelerine mesajı gönder
            var usernames = _context.GroupMembers
                .Where(gm => gm.GroupId == request.GroupId)
                .Select(gm => gm.User.Username)
                .ToList();

            foreach (var username in usernames)
            {
                var user = ConnectedUserHandler.GetUserByUsername(username);
                if (user != null)
                {
                    await _hubContext.Clients.Client(user.ConnectionId)
                        .SendAsync("ReceiveGroupMessage", senderUsername, request.Msg, request.GroupId, DateTime.Now.ToString("HH:mm"));
                }
            }
        }

        public async Task<List<GroupMessageResponse>> GetGroupMessagesAsync(int groupId)
        {
            return await _context.GroupMessages
                .Include(m => m.SenderUser)
                .Where(m => m.GroupId == groupId)
                .OrderBy(m => m.SentAt)
                .Select(m => new GroupMessageResponse
                {
                    SenderUsername = m.SenderUser.Username,
                    Message = m.Msg,
                    Time = m.SentAt.ToString("HH:mm")
                })
                .ToListAsync();
        }



    }
}
