using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Auth
{
    public class ApplicationUser
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class MobileApplicationUser : ApplicationUser
    {
        [Required]
        public string DeviceName { get; set; }
    }
}
