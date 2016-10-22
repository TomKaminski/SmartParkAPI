using System;
using System.Collections.Generic;
using SmartParkAPI.Models.Base;
using SmartParkAPI.Models.Portal.User;

namespace SmartParkAPI.Models.Admin.User
{
    public class AdminUserListItemViewModel: SmartParkListBaseViewModel
    {
        public int Id { get; set; }
        public string Initials { get; set; }
        public string CreateDateLabel { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public int OrdersCount { get; set; }
        public IEnumerable<ShopOrderItemViewModel> LastUserOrders { get; set; }
        public int Charges { get; set; }
        public int GateUsagesCount { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string ImgId { get; set; }
        public string Email { get; set; }
    }
}
