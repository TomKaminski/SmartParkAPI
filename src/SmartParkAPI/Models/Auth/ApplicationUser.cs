namespace SmartParkAPI.Models.Auth
{
    public class ApplicationUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class MobileApplicationUser : ApplicationUser
    {
        public string DeviceName { get; set; }
    }
}
