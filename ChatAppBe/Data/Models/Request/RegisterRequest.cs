using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatAppBe.Data.Models.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3 ile 20 karakter arasında olmalıdır.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Şifre 3 ile 100 karakter arasında olmalıdır.")]
        public string Password { get; set; }
    }
}
