using ChatAppBe.Data.Models.Request;
using ChatAppBe.Data.Models.Response;

namespace ChatAppBe.Services
{
    public interface IUserService
    {
        int Register(RegisterRequest user);
        int Login(LoginRequest request);
        List<UserResponse> GetAllUsers();
    }
}
