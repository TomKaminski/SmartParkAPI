using System;
using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Portal.PortalMessage
{
    public class SetDisplayedMessageViewModel
    {
        [Required]
        public Guid MessageId { get; set; }
    }
}