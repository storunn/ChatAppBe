using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Models.Request;
using ChatAppBe.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}

