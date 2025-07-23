using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Entities;
using ChatAppBe.Data.Models.Request;

namespace ChatAppBe.Services;
public class MessageService : IMessageService
{

    private readonly AppDbContext _context;

    public MessageService(AppDbContext context)
    {
        _context = context;
    }

    public bool SendMessage(SendMessageRequest request)
    {
        _context.Messages.Add(new Message
        {
            SenderUserId = request.SenderUserId,
            ReceiverUserId = request.ReceiverUserId,
            Msg = request.Msg,
        });
        _context.SaveChanges();
        return true;
    }
}