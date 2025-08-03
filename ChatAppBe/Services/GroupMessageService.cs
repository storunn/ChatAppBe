using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Entities;
using ChatAppBe.Data.Models.Request;
using ChatAppBe.Handlers;
using ChatAppBe.Hubs;
using ChatAppBe.Services;
using Microsoft.AspNetCore.SignalR;

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

        // Grubun tüm üyelerine mesaj gönder
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
                    .SendAsync("ReceiveGroupMessage", request.SenderUserId, request.Msg, request.GroupId, DateTime.Now.ToString("HH:mm"));
            }
        }
    }
}
