using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Portal.Account
{
    public class ForgotPasswordViewModel : SmartParkBaseViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [EmailAddress(ErrorMessage = "To nie jest adres email")]
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}