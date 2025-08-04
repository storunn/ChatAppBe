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

        //// 📥 Veritabanına mesajı ekle
        //var message = new Message
        //{
        //    SenderUserId = senderUser.Id,
        //    ReceiverUserId = request.ReceiverUserId, // varsa
        //    Msg = request.Msg,
        //    SentAt = DateTime.Now
        //};

        //_context.Messages.Add(message);
        //Console.WriteLine("[DEBUG] SendMessageAsync tetiklendi: " + request.Msg);
        //try
        //{
        //    await _context.SaveChangesAsync();
        //}
        //catch (DbUpdateException ex)
        //{
        //    Console.WriteLine("Veritabanı hatası: " + ex.Message);
        //    if (ex.InnerException != null)
        //        Console.WriteLine("Inner exception: " + ex.InnerException.Message);
        //    return false;
        //}

        // 🔔 Mesajı kullanıcıya gönder
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
}
