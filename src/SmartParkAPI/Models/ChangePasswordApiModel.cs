using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models
{
    public class ChangePasswordApiModel
    {
        [Required]
        public string Email { get; set; }

        [Required, Compare("NewPassword")]
        public string NewPasswordRepeat { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string OldPassword { get; set; }
    }
}
