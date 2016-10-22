using System;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Models.Portal.PortalMessage
{
    public class PortalMessageItemViewModel
    {
        public Guid Id { get; set; }
        public string CreateDate { get; set; }
        public string Text { get; set; }
        public bool ToAdmin { get; set; }
        public bool IsNotification { get; set; }
        public PortalMessageEnum PortalMessageType { get; set; }
        public bool IsDisplayed { get; set; }
        public bool Starter { get; set; }
        public string Title { get; set; }

        public bool HiddenForReceiver { get; set; }
        public bool HiddenForSender { get; set; }

        public Guid? PreviousMessageId { get; set; }
        public int UserId { get; set; }
        public int ReceiverUserId { get; set; }

        public bool OutMessage { get; set; }
    }
}
