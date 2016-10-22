using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Base
{
    public class SmartParkEditBaseViewModel<T> : SmartParkBaseViewModel
        where T : struct
    {
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        public T Id { get; set; }
    }
}
