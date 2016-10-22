using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Parking
{
    public class RefreshChargesApiModel
    {
        [Required]
        public string Email { get; set; }
    }
}
