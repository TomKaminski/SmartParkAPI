using System.Collections.Generic;

namespace SmartParkAPI.Models.Portal.Payment
{
    public class Buyer
    {
        public string email { get; set; }
        public string phone { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string language { get; set; }
    }

    public class PayMethod
    {
        public string type { get; set; }
    }

    public class Product
    {
        public string name { get; set; }
        public string unitPrice { get; set; }
        public string quantity { get; set; }
    }

    public class Order
    {
        public string orderId { get; set; }
        public string extOrderId { get; set; }
        public string orderCreateDate { get; set; }
        public string notifyUrl { get; set; }
        public string customerIp { get; set; }
        public string merchantPosId { get; set; }
        public string description { get; set; }
        public string currencyCode { get; set; }
        public string totalAmount { get; set; }
        public Buyer buyer { get; set; }
        public PayMethod payMethod { get; set; }
        public List<Product> products { get; set; }
        public string status { get; set; }
    }

    public class PayuNotificationModel
    {
        public Order order { get; set; }
    }
}
