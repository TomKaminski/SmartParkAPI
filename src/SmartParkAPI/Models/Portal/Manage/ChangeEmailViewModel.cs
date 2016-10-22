using System.ComponentModel.DataAnnotations;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Portal.Manage
{
    public class ChangeEmailViewModel : SmartParkBaseViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [EmailAddress]
        public string NewEmail { get; set; }

        [Compare("NewEmail")]
        public string NewEmailRepeat { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        public string Password { get; set; }
    }
}