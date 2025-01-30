using System.ComponentModel.DataAnnotations;

namespace Univent.App.Authentication.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
