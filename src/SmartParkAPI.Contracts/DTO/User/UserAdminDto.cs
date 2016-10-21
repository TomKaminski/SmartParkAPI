using System.Collections.Generic;
using SmartParkAPI.Contracts.DTO.Order;

namespace SmartParkAPI.Contracts.DTO.User
{
    public class UserAdminDto : UserBaseDto
    {
        public int OrdersCount { get; set; }
        public int GateUsagesCount { get; set; }
        public IEnumerable<OrderBaseDto> Orders { get; set; }
        public string ImgId { get; set; }
    }
}
