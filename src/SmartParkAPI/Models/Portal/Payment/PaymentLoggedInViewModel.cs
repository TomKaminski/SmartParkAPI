using System.ComponentModel.DataAnnotations;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Portal.Payment
{
    public class PaymentLoggedInViewModel : SmartParkBaseViewModel
    {
        [Required]
        public int Charges { get; set; }

    }
}
