using System.Collections.Generic;

namespace SmartParkAPI.Contracts.DTO.Payments
{
    public class Buyer
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string language => "pl";
    }

    public class Product
    {
        public string name { get; set; }
        public string unitPrice { get; set; }
        public string quantity { get; set; }
    }

    public class PaymentRequest
    {
        public string notifyUrl { get; set; }
        public string customerIp { get; set; }
        public string merchantPosId { get; set; }
        public string description { get; set; }
        public string continueUrl { get; set; }

        public string currencyCode => "PLN";

        public string totalAmount { get; set; }
        public string extOrderId { get; set; }
        public Buyer buyer { get; set; }
        public List<Product> products { get; set; }
    }

    public class PaymentCardRequest
    {
        public string notifyUrl { get; set; }
        public string customerIp { get; set; }
        public string merchantPosId { get; set; }
        public string description { get; set; }
        public string currencyCode => "PLN";
        public string totalAmount { get; set; }
        public string extOrderId { get; set; }
        public List<Product> products { get; set; }
        public Buyer buyer { get; set; }
        public PayMethods payMethods { get; set; }
        public string deviceFingerprint { get; set; }

    }

    public class PayMethod
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class PayMethods
    {
        public PayMethod payMethod { get; set; }
    }
}
