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
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromQuery] string name, [FromQuery] int creatorUserId)
        {
            var groupId = await _groupService.CreateGroupAsync(name, creatorUserId); // Creator'ı otomatik ekleyecek!
            return Ok(new { GroupId = groupId });
        }

        // 2. Gruba Kullanıcı Ekleme
        [HttpPost]
        public async Task<IActionResult> AddUserToGroup([FromQuery] int groupId, [FromQuery] int userId)
        {
            var result = await _groupService.AddUserToGroupAsync(groupId, userId);
            return result ? Ok("Kullanıcı eklendi.") : BadRequest("Zaten bu grupta.");
        }


        // 3. Kullanıcının Gruplarını Listeleme
        [HttpGet]
        public async Task<ActionResult<List<GroupResponse>>> GetGroupMembers(int userId)
        {
            var groups = await _groupService.GetGroupMembersAsync(userId);
            return Ok(groups);
        }
        [HttpPost]
        public async Task<IActionResult> LeaveGroup([FromQuery] int groupId, [FromQuery] int userId)
        {
            var result = await _groupService.LeaveGroupAsync(groupId, userId);
            return result ? Ok("Gruptan çıkıldı.") : BadRequest("Çıkma işlemi başarısız.");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromGroup([FromQuery] int groupId, [FromQuery] int userId)
        {
            var result = await _groupService.RemoveUserFromGroupAsync(groupId, userId);
            return result ? Ok("Kullanıcı gruptan çıkarıldı.") : BadRequest("Çıkarma işlemi başarısız.");
        }
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetMembersOfGroup([FromQuery] int groupId)
        {
            var members = await _groupService.GetMembersOfGroupAsync(groupId);
            return Ok(members);
        }



    }
}
