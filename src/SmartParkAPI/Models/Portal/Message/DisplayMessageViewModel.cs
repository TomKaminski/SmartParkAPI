using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Portal.Message
{
    public class DisplayMessageViewModel:SmartParkBaseViewModel
    {
        public string Title { get; set; }
        public string EmailHtml { get; set; }
    }
}
