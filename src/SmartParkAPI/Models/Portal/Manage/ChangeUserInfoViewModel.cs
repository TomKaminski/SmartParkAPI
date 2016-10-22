using System.ComponentModel.DataAnnotations;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Portal.Manage
{
    public class ChangeUserInfoViewModel : SmartParkBaseViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        public string LastName { get; set; }
    }
}