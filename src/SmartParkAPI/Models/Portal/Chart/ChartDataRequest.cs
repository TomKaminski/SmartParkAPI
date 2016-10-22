using System;
using System.ComponentModel.DataAnnotations;
using SmartParkAPI.Infrastructure.Attributes;

namespace SmartParkAPI.Models.Portal.Chart
{
    public class ChartDataRequest
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        [IsDateAfter("StartDate", true, ErrorMessage = "Data końcowa musi być równa lub późniejsza od daty początkowej.")]
        public DateTime EndDate { get; set; }

        [Required]
        public int Granuality { get; set; }

        [Required]
        public int Type { get; set; }
    }
}
