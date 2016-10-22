using System;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Admin.GateUsage
{
    public class AdminGateUsageListItemViewModel : SmartParkListBaseViewModel
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Initials { get; set; }
    }
}
