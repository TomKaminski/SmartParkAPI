using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Auth
{
    public class RefreshAppTokenModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
