using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Panel
{
    public class CheckAccountApiModel
    {
        [Required]
        public string Email { get; set; }
    }
}
