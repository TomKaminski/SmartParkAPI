using System.Collections.Generic;

namespace SmartParkAPI.Models.Portal.PortalMessage
{
    public class PortalMessageClustersViewModel
    {
        public PortalMessageUserViewModel User { get; set; }
        public IEnumerable<PortalMessageClusterViewModel> Clusters { get; set; }
    }

    public class PortalMessageClusterViewModel
    {
        public PortalMessageItemViewModel[] Messages { get; set; }
        public PortalMessageUserViewModel ReceiverUser { get; set; }
    }
}
