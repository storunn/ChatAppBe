using ChatAppBe.Data.Models.Response;
using ChatAppBe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ChatAppBe.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        
    private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        // 1. Grup Oluşturma
        [HttpPost("CreateGroup")]

        public async Task<IActionResult> CreateGroup([FromQuery] string name)
        {
            var groupId = await _groupService.CreateGroupAsync(name);
            return Ok(new { GroupId = groupId });
        }

        // 2. Gruba Kullanıcı Ekleme
        [HttpPost("adduser")]
        public async Task<IActionResult> AddUserToGroup([FromQuery] int groupId, [FromQuery] int userId)
        {
            var result = await _groupService.AddUserToGroupAsync(groupId, userId);
            return result ? Ok("Kullanıcı eklendi.") : BadRequest("Zaten bu grupta.");
        }

        // 3. Kullanıcının Gruplarını Listeleme
        [HttpGet("GetGroupMembers/{userId}")]
        public async Task<ActionResult<List<GroupResponse>>> GetGroupMembers(int userId)
        {
            var groups = await _groupService.GetGroupMembersAsync(userId);
            return Ok(groups);
        }

    }
}
