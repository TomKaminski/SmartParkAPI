namespace SmartParkAPI.Models.Portal
{
    public class HomeViewModel
    {
        public int UnreadClustersCount { get; set; }
        public bool FromShop { get; set; }
        public bool IsError { get; set; }
        public string PathBase { get; set; }
    }
}
