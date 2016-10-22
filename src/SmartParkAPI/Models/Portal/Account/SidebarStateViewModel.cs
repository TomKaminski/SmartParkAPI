using System.ComponentModel.DataAnnotations;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Portal.Account
{
    public class SidebarStateViewModel : SmartParkBaseViewModel
    {
        [Required]
        public bool SidebarShrinked { get; set; }
    }
}
