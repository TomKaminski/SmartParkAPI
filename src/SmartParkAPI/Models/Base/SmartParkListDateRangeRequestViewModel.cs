using System;
using System.ComponentModel.DataAnnotations;

namespace SmartParkAPI.Models.Base
{
    public class SmartParkListDateRangeRequestViewModel
    {
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
    }
}