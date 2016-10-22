﻿using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Models.Admin.Order
{
    public class AdminOrderListItemViewModel : SmartParkListBaseViewModel
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public int NumOfCharges { get; set; }
        public string Price { get; set; }
        public string PricePerCharge { get; set; }
        public string OrderPlace { get; set; }
        public string OrderState { get; set; }
        public string OrderStateStyle { get; set; }
        public string Initials { get; set; }
    }
}
