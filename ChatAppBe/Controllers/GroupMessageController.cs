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

    [HttpPost]
    public async Task<IActionResult> SendGroupMessage([FromBody] SendGroupMessageRequest request)
    {
        await _groupMessageService.SendGroupMessageAsync(request);
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> GetGroupMessages([FromQuery] int groupId)
    {
        var messages = await _groupMessageService.GetGroupMessagesAsync(groupId);
        return Ok(messages);
    }

}

