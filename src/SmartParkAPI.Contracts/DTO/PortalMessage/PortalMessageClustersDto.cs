using System.Collections.Generic;

namespace SmartParkAPI.Contracts.DTO.PortalMessage
{
    public class PortalMessageClustersDto
    {
        public PortalMessageUserDto User { get; set; }
        public List<PortalMessageClusterDto> Clusters { get; set; }
    }
}
