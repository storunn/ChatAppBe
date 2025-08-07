using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Models.Request;
using ChatAppBe.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBe.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

      
        public UserController(AppDbContext context, IUserService userService)
        {
            _context = context;
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
        public IActionResult Search(string query)
        {
            var users = _context.Users
                .Where(u => u.Username.Contains(query))
                .Select(u => new { u.Username, u.Id })
                .ToList();
            return Ok(users);
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }
    }
}
