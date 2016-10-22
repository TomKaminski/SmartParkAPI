using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Panel
{
    public class PaymentRequestApiModel
    {
        [Required]
        public int Charges { get; set; }
        public string CustomerIP { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserLastName { get; set; }
        [Required]
        public string CardTokenValue { get; set; }
        [Required]
        public string DeviceFingerPrint { get; set; }
    }
}