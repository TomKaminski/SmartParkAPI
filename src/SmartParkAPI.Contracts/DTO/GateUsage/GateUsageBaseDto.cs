using System;
using SmartParkAPI.Contracts.Common;

namespace SmartParkAPI.Contracts.DTO.GateUsage
{
    public class GateUsageBaseDto:BaseDto<Guid>
    {
        public DateTime DateOfUse { get; set; }
        public int UserId { get; set; }
    }
}
