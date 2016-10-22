using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Account
{
    public class LoginApiModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
