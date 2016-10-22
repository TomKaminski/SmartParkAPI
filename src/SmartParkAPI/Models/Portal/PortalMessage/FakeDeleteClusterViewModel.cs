using System;
using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Portal.PortalMessage
{
    public class FakeDeleteClusterViewModel
    {
        [Required]
        public Guid StarterMessageId { get; set; }
    }
}