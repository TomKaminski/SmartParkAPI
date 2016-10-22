namespace SmartParkAPI.Models.Portal.User
{
    public class UserBaseViewModel
    {
        public string Email { get; set; }
        public int Charges { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CreateDate { get; set; }
        public string Range { get; set; }
        public string LastGateOpenDate { get; set; }
    }
}
