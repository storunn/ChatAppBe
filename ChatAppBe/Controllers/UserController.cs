using ChatAppBe.Data.Models.Request;
using ChatAppBe.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBe.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Register(RegisterRequest request)
        {
            return Ok(_userService.Register(request));
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            var response = _userService.Login(request);
            if (response == -1)
                return Ok("Kullanıcı bulunamadı!");
            else
                return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }
    }
}
