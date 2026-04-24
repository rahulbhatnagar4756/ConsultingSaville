using System.ComponentModel.DataAnnotations;

namespace Library.Base.Models.Users
{
    public class UserLoginRequestModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public string? CompanyUUID { get; set; }
    }
}