using System;
using System.ComponentModel.DataAnnotations;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Models.Portal.PortalMessage
{
    public class ReplyMessageViewModel
    {
        [Required(ErrorMessage = "Treść wiadomości nie może być pusta!")]
        public string Text { get; set; }
        public bool ToAdmin { get; set; }
        public bool IsNotification => false;
        public PortalMessageEnum PortalMessageType { get; set; }
        public bool IsDisplayed => false;
        [Required(ErrorMessage = "Nie znaleziono poprzedniej wiadomości")]
        public Guid PreviousMessageId { get; set; }
        public DateTime CreateDate => DateTime.Now;
        public int UserId { get; set; }
        public int ReceiverUserId { get; set; }
    }
}
