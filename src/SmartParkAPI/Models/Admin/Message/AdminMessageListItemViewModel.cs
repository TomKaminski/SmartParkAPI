using System;
using SmartParkAPI.Models.Base;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Models.Admin.Message
{
    public class AdminMessageListItemViewModel : SmartParkListBaseViewModel
    {
        public Guid Id { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Title { get; set; }
        public string MessageParameters { get; set; }
        public EmailType Type { get; set; }
        public string DisplayFrom { get; set; }
        public string From { get; set; }
    }
}
