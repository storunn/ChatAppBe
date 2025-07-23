using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Entities;
using ChatAppBe.Data.Models.Request;
using ChatAppBe.Data.Models.Response;

namespace ChatAppBe.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public int Register(RegisterRequest user)
        {
            // Kullanıcı var mı kontrolü
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
            {
                return -1; // Kullanıcı zaten var
            }

            var newUser = new User
            {
                Username = user.Username,
                Password = user.Password,
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return newUser.Id;
        }

        public int Login(LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == request.Username && x.Password == request.Password);
            if (user == null)
            {
                return -1;
            }

            return user.Id;
        }

        public List<UserResponse> GetAllUsers()
        {
            return _context.Users.Select(x => new UserResponse
            {
                Id = x.Id,
                Username = x.Username,
            }).ToList();
        }
    }
}
