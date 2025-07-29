using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Entities;
using ChatAppBe.Data.Models.Request;
using ChatAppBe.Data.Models.Response;
using ChatAppBe.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppBe.Services;
public class MessageService : IMessageService
{

    private readonly AppDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageService(AppDbContext context, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<bool> SendMessageAsync(SendMessageRequest request)
    {
        User? senderUser;

        if (request.SenderUserId.HasValue)
            senderUser = _context.Users.Where(x => x.Id == request.SenderUserId).FirstOrDefault();
        else
            senderUser = _context.Users.Where(x => x.Username == request.SenderUsername).FirstOrDefault();

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", senderUser.Username, request.Msg);

        return true;
    }

    public List<MessageResponse> GetMessagesByUserId(int userId)
    {
        return _context.Messages.Where(x => x.SenderUserId == userId || x.ReceiverUserId == userId).Select(x => new MessageResponse
        {
            Id = x.Id,
            SenderUserId = x.SenderUserId,
            SenderUsername = x.SenderUser.Username,
            ReceiverUserId = x.ReceiverUserId,
            ReceiverUsername = x.ReceiverUser.Username,
            Msg = x.Msg,
            SentAt = x.SentAt,
        }).ToList();
    }

    public List<MessageResponse> GetMessagesByUsername(string username)
    {
        var receiverUser = _context.Users.Where(x => x.Username == username).FirstOrDefault();
        if (receiverUser != null)
        {
            return _context.Messages.Where(x => x.SenderUserId == receiverUser.Id || x.ReceiverUserId == receiverUser.Id)
                .Select(x => new MessageResponse
                {
                    Id = x.Id,
                    SenderUserId = x.SenderUserId,
                    SenderUsername = x.SenderUser.Username,
                    ReceiverUserId = x.ReceiverUserId,
                    ReceiverUsername = x.ReceiverUser.Username,
                    Msg = x.Msg,
                    SentAt = x.SentAt,
                }).ToList();
        }

        return new List<MessageResponse>() { };
    }
}
