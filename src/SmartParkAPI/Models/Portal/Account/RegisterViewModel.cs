using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Portal.Account
{
    public class RegisterViewModel : SmartParkBaseViewModel
    {
        [EmailAddress]
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [Display(Name = "e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [MinLength(8)]
        [PasswordPropertyText]
        [Display(Name = "hasło")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [Compare("Password")]
        [PasswordPropertyText]
        [Display(Name = "powtórz hasło")]
        public string RepeatPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [Display(Name = "imię")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [Display(Name = "nazwisko")]
        public string LastName { get; set; }
    }
}