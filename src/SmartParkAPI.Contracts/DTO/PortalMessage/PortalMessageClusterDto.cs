namespace SmartParkAPI.Contracts.DTO.PortalMessage
{
    public class PortalMessageClusterDto
    {
        public PortalMessageUserDto ReceiverUser { get; set; }
        public PortalMessageDto[] Cluster { get; set; }
    }
}