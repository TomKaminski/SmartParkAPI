using System.ComponentModel.DataAnnotations;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Portal.Manage
{
    public class SendChargesViewModel : SmartParkBaseViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [EmailAddress]
        public string ReceiverEmail { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [Range(0, 99999)]
        public int AmountOfCharges { get; set; }
    }
}
