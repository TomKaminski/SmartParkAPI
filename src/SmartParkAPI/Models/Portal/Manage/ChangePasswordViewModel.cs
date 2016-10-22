using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Portal.Manage
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        public string OldPassword { get; set; }
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        public string Password { get; set; }
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [Compare("Password", ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "ResetPasswordViewModel_Password_CompareError")]
        public string RepeatPassword { get; set; }
    }
}
