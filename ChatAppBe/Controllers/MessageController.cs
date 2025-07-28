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

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public IActionResult SendMessage(SendMessageRequest request)
        {
            return Ok(_messageService.SendMessageAsync(request));
        }

        //[HttpGet]
        //public IActionResult GetMessagesByUserId(int userId)
        //{
        //    return Ok(_messageService.GetMessagesByUserId(userId));
        //}

        //[HttpGet]
        //public IActionResult GetMessagesByUsername(string username)
        //{
        //    return Ok(_messageService.GetMessagesByUsername(username));
        //}
    }
}
