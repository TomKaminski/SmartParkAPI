namespace SmartParkAPI.Contracts.DTO.Payments
{
    public class Status
    {
        public string statusCode { get; set; }
    }

    public class PaymentResponse
    {
        public Status status { get; set; }
        public string redirectUri { get; set; }
        public string orderId { get; set; }
        public string extOrderId { get; set; }
    }



    public class Card
    {
        public string number { get; set; }
        public string expirationMonth { get; set; }
        public string expirationYear { get; set; }
    }

    public class CardPayMethod
    {
        public Card card { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class CardPayMethods
    {
        public CardPayMethod payMethod { get; set; }
    }

    public class CardStatus
    {
        public string statusCode { get; set; }
        public string statusDesc { get; set; }
    }

    public class PaymentCardResponse
    {
        public string orderId { get; set; }
        public CardPayMethods payMethods { get; set; }
        public CardStatus status { get; set; }
    }
}
