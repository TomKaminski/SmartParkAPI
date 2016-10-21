using System;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Contracts.DTO.Order
{
    public class OrderBaseDto : BaseDto<long>
    {
        public decimal Price { get; set; }
        public decimal PricePerCharge { get; set; }
        public int NumOfCharges { get; set; }
        public Guid ExtOrderId { get; set; }
        public OrderPlace OrderPlace { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus OrderState { get; set; }

        public int UserId { get; set; }
        public int PriceTresholdId { get; set; }
    }
}
