using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Admin.User
{
    public class AdminUserEditViewModel : SmartParkEditBaseViewModel<int>
    {
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [DisplayName("Imię")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [DisplayName("Nazwisko")]
        public string LastName { get; set; }

        [Required]
        public int Charges { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string OldEmail { get; set; }
    }
}
