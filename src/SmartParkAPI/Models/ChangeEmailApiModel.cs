using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models
{
    public class ChangeEmailApiModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string NewEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
