using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Parking
{
    public class OpenGateApiModel
    {
        [Required]
        public string Email { get; set; }
    }
}
