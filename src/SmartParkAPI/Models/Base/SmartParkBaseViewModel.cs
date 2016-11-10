using System.Collections.Generic;
using System.Linq;

namespace SmartParkAPI.Models.Base
{
    public abstract class SmartParkBaseViewModel
    {
        //protected SmartParkBaseViewModel()
        //{
        //    ValidationErrors = new List<string>();
        //    SuccessNotifications = new List<string>();
        //}
        //public List<string> ValidationErrors { get; set; }
        //public List<string> SuccessNotifications { get; set; }

        //public bool IsValid => ValidationErrors == null || !ValidationErrors.Any();

        //public void AppendErrors(IEnumerable<string> errors)
        //{
        //    ValidationErrors.AddRange(errors);
        //}

        //public void AppendNotifications(IEnumerable<string> notifications)
        //{
        //    SuccessNotifications.AddRange(notifications);
        //}

        //public void AppendNotifications(params string[] notifications)
        //{
        //    SuccessNotifications.AddRange(notifications);
        //}
    }
}
