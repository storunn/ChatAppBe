using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Models.Request;
using ChatAppBe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBe.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly AppDbContext _context;

        public MessageController(IMessageService messageService, AppDbContext context)
        {
            _messageService = messageService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var success = await _messageService.SendMessageAsync(request);

            if (success)
                return Ok(new { Message = "Mesaj gönderildi." });
            else
                return StatusCode(500, new { Error = "Mesaj gönderilemedi." });
        }

        [HttpGet("inbox/{userId}")]
        public IActionResult GetInbox(int userId)
        {
            var messages = _context.Messages
                .Where(m => m.ReceiverUserId == userId || m.ReceiverUserId == null)
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    From = _context.Users.FirstOrDefault(u => u.Id == m.SenderUserId).Username,
                    Msg = m.Msg,
                    Time = m.SentAt.ToString("HH:mm")
                })
                .ToList();

            return Ok(messages);
        }
        [HttpGet]
        public async Task<IActionResult> GetPrivateMessages(string username1, string username2)
        {
            var messages = await _messageService.GetPrivateMessagesAsync(username1, username2);
            return Ok(messages);
        }
        [HttpGet]
        public IActionResult GetMyChats(int userId)
        {
            var myChats = _context.Messages
                .Where(m => m.SenderUserId == userId || m.ReceiverUserId == userId)
                .Select(m => m.SenderUserId == userId ? m.ReceiverUser.Username : m.SenderUser.Username)
                .Distinct()
                .ToList();
            return Ok(myChats);
        }

    }
}

