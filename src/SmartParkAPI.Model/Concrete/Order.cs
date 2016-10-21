using System;
using SmartParkAPI.Model.Common;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Model.Concrete
{
    public class Order : Entity<long>
    {
        public decimal Price { get; set; }
        public int NumOfCharges { get; set; }
        public Guid ExtOrderId { get; set; }
        public OrderPlace OrderPlace { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus OrderState { get; set; }

        public int UserId { get; set; }
        public int PriceTresholdId { get; set; }

        public virtual PriceTreshold PriceTreshold {get; set; }
        public virtual User User { get; set; }
    }
}
