using System.ComponentModel.DataAnnotations.Schema;
using SmartParkAPI.Model.Common;

namespace SmartParkAPI.Model.Concrete
{
    public class UserDevice : Entity<int>
    {
        public string Name { get; set; }
        public string Token { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
