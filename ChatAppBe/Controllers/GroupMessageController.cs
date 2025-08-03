using ChatAppBe.Data.Models.Request;
using ChatAppBe.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupMessageController : ControllerBase
{
    private readonly IGroupMessageService _groupMessageService;

    public GroupMessageController(IGroupMessageService groupMessageService)
    {
        _groupMessageService = groupMessageService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendGroupMessage([FromBody] SendGroupMessageRequest request)
    {
        await _groupMessageService.SendGroupMessageAsync(request);
        return Ok();
    }
}