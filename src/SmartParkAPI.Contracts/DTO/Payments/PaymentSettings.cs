namespace SmartParkAPI.Contracts.DTO.Payments
{
    public class PaymentSettings
    {
        public string HostAddress { get; set; }
        public string OrderCreateEndpoint { get; set; }
        public string AuthorizeEndpoint { get; set; }
        public string PosID { get; set; }
        public string OAuthClientSecret { get; set; }
        public string ClientSecondKey { get; set; }
    }
}