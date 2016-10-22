using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Portal.GateUsage
{
    public class GateOpeningViewModel:SmartParkListBaseViewModel
    {
        public string Date { get; set; }
        public string Time { get; set; }
    }
}