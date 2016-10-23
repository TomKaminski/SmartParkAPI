using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingATHWeb.Shared.Enums;
using SmartParkAPI.Contracts.DTO.Chart;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Models.Portal.Chart;
using SmartParkAPI.Models.Portal.Weather;
using SmartParkAPI.Shared.Enums;

namespace SmartParkAPI.Controllers.Portal
{
    [Area("Portal")]
    [Route("[area]")]
    [Authorize]
    public class HomeController : BaseApiController
    {
        private readonly IWeatherService _weatherService;
        private readonly IUserService _userService;
        private readonly IGateUsageService _gateUsageService;
        private readonly IMapper _mapper;
        private readonly IChartService _chartService;
        private readonly IPortalMessageService _portalMessageService;

        public HomeController(IWeatherService weatherService, IUserService userService, IGateUsageService gateUsageService, IMapper mapper, IChartService chartService, IPortalMessageService portalMessageService)
        {
            _weatherService = weatherService;
            _userService = userService;
            _gateUsageService = gateUsageService;
            _mapper = mapper;
            _chartService = chartService;
            _portalMessageService = portalMessageService;
        }

        //[Route("~/[area]")]
        //public async Task<IActionResult> Index(bool fromShop = false, bool isError = false, string pathBase = null)
        //{
        //    var unreadClusters =
        //        (await _portalMessageService.GetUnreadClustersCountAsync(CurrentUser.UserId.Value)).Result;

        //    return View(new HomeViewModel
        //    {
        //        FromShop = fromShop,
        //        UnreadClustersCount = unreadClusters,
        //        IsError = isError,
        //        PathBase = GetPathBaseRedirect(pathBase)
        //    });
        //}

        [Route("[controller]/[action]")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Json(new
            {
                asd = 5
            });
        }

        [Route("~/Thanks")]
        public IActionResult ShopContinue(string error = null)
        {
            return RedirectToActionPermanent("Index", new { fromShop = true, isError = error != null });
        }

        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetWeatherData()
        {
            var weatherData = _mapper.Map<WeatherDataViewModel>((await _weatherService.GetLatestWeatherDataAsync()).Result);
            return Json(weatherData);
        }

        //[Route("[controller]/[action]")]
        //public async Task<IActionResult> GetUserChargesData()
        //{
        //    var user = (await _userService.GetByEmailAsync(CurrentUser.Email)).Result;
        //    var userId = user.Id;
        //    var endDate = DateTime.Today.AddDays(1).AddSeconds(-1);
        //    var startDate = DateTime.Today.AddDays(-6);
        //    var userGateUsages = (await _gateUsageService.GetAllAsync(x => x.UserId == userId)).Result.ToList();

        //    var lineChartData = await _chartService.GetDataAsync(new ChartRequestDto(startDate, endDate, ChartType.GateOpenings, ChartGranuality.PerDay, userId));

        //    return Json(new
        //    {
        //        chargesUsed = userGateUsages.Count,
        //        chargesLeft = user.Charges,
        //        lineChartData = _mapper.Map<ChartDataReturnModel>(lineChartData.Result)
        //    });
        //}

        private string GetPathBaseRedirect(string pathBase = null)
        {
            switch (pathBase)
            {
                case "Dashboard":
                    return string.Empty;
                case "Konto":
                    return "account";
                case "Sklep":
                    return "sklep";
                case "Statystyki":
                    return "statistics";
                case "Wiadomosci":
                    return "messages";
                case "Uzytkownicy":
                    return "adminUsers";
                case "Zamowienia":
                    return "adminOrders";
                case "Cennik":
                    return "adminPrices";
                case "Wyjazdy":
                    return "adminGateusages";
                default:
                    return string.Empty;
            }
        }
    }
}
