using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Account
{
    public class ForgotApiModel
    {
        [Required]
        public string Email { get; set; }
    }
}
