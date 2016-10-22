using System;
using System.Collections.Generic;

namespace SmartParkAPI.Models.Portal.Message
{
    public class MessageClusterListViewModel
    {
        public int TotalClustersCount { get; set; }
        public int ReturnedClustersCount { get; set; }

        public IEnumerable<MessageCluster> MessageClusters { get; set; }
    }

    public class MessageCluster
    {
        public MessageCluster Parent { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string ReceiverUserName { get; set; }
        public string SenderUserName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDateLabel { get; set; }

        public Guid? SenderProfileImageGuid { get; set; }
        public Guid? ReceiverProfileImageGuid { get; set; }
    }
}
