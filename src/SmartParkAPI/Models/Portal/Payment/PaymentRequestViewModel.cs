using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Portal.Payment
{
    public class PaymentRequestViewModel
    {
        [Required]
        public int Charges { get; set; }
        public string CustomerIP { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
    }
}
