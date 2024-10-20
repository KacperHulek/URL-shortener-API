using System.ComponentModel.DataAnnotations;

namespace URL_shortener_API.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
