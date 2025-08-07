using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Entities;
using ChatAppBe.Data.Models;
using ChatAppBe.Data.Models.Request;
using ChatAppBe.Data.Models.Response;
using ChatAppBe.Handlers;
using ChatAppBe.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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
            senderUser = _context.Users.FirstOrDefault(x => x.Id == request.SenderUserId.Value);
        else
            senderUser = _context.Users.FirstOrDefault(x => x.Username == request.SenderUsername);

        if (senderUser == null)
        {
            Console.WriteLine("Kullanıcı bulunamadı!");
            return false;
        }

        var messageResponse = new MessageResponse
        {
            SenderUserId = senderUser.Id,
            SenderUsername = senderUser.Username,
            Msg = request.Msg,
            SentAt = DateTime.Now
        };
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", messageResponse);

        return true;


    }


    public List<MessageResponse> GetMessagesByUserId(int userId)
    {
        return _context.Messages.Where(x => x.SenderUserId == userId || x.ReceiverUserId == userId).Select(x => new MessageResponse
        {
            Id = x.Id,
            SenderUserId = x.SenderUserId,
            SenderUsername = x.SenderUser.Username,
            ReceiverUserId = (int)x.ReceiverUserId,
            ReceiverUsername = x.ReceiverUser.Username,
            Msg = x.Msg,
            SentAt = DateTime.Now
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
                    ReceiverUserId = (int)x.ReceiverUserId,
                    ReceiverUsername = x.ReceiverUser.Username,
                    Msg = x.Msg,
                    SentAt = DateTime.Now
                }).ToList();
        }

        return new List<MessageResponse>() { };
    }
    public async Task<List<PrivateMessageResponse>> GetPrivateMessagesAsync(string username1, string username2)
    {
        return await _context.Messages
            .Include(m => m.SenderUser)
            .Include(m => m.ReceiverUser)
            .Where(m =>
                (m.SenderUser.Username == username1 && m.ReceiverUser.Username == username2) ||
                (m.SenderUser.Username == username2 && m.ReceiverUser.Username == username1))
            .OrderBy(m => m.SentAt)
            .Select(m => new PrivateMessageResponse
            {
                From = m.SenderUser.Username,
                Message = m.Msg,
                Time = m.SentAt.ToString("HH:mm")
            })
            .ToListAsync();
    }

    
}
