using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingATHWeb.Shared.Enums;
using SmartParkAPI.Contracts.DTO.Chart;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Models.Base;
using SmartParkAPI.Models.Portal.Chart;
using SmartParkAPI.Models.Portal.GateUsage;
using SmartParkAPI.Models.Portal.User;

namespace SmartParkAPI.Controllers.Portal
{
    [Area("Portal")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class StatisticsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IChartService _chartService;
        private readonly IOrderService _orderService;
        private readonly IGateUsageService _gateUsageService;

        public StatisticsController(IChartService chartService, IMapper mapper, IGateUsageService gateUsageService, IOrderService orderService)
        {
            _chartService = chartService;
            _mapper = mapper;
            _gateUsageService = gateUsageService;
            _orderService = orderService;
        }

        [Route("GetChartData")]
        [HttpPost]
        public async Task<IActionResult> GetChartData([FromBody]ChartDataRequest model)
        {
            if (ModelState.IsValid)
            {
                var serviceRequest = _mapper.Map<ChartRequestDto>(model);

                // ReSharper disable once PossibleInvalidOperationException
                serviceRequest.UserId = CurrentUser.UserId.Value;

                var chartDataResult = await _chartService.GetDataAsync(serviceRequest);
                if (chartDataResult.IsValid)
                {
                    return Ok(SmartJsonResult<ChartDataReturnModel>.Success(_mapper.Map<ChartDataReturnModel>(chartDataResult.Result)));
                }

                return BadRequest(SmartJsonResult.Failure(chartDataResult.ValidationErrors));
            }
            return BadRequest(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        }

        [Route("GetDefaultChartData")]
        [HttpPost]
        public async Task<IActionResult> GetDefaultChartData()
        {
            if (ModelState.IsValid)
            {
                // ReSharper disable once PossibleInvalidOperationException
                var chartDataAndPreferencesResult = await _chartService.GetDefaultDataAsync(CurrentUser.UserId.Value);
                if (chartDataAndPreferencesResult.IsValid)
                {
                    var result = new
                    {
                        gateUsagesData = _mapper.Map<ChartDataReturnModel>(chartDataAndPreferencesResult.Result.FirstOrDefault(x => x.Key == ChartType.GateOpenings).Value),
                        ordersData = _mapper.Map<ChartDataReturnModel>(chartDataAndPreferencesResult.Result.FirstOrDefault(x => x.Key == ChartType.Orders).Value),
                    };

                    return Ok(SmartJsonResult<object, ChartPreferencesReturnModel>.Success(result, _mapper.Map<ChartPreferencesReturnModel>(chartDataAndPreferencesResult.SecondResult)));
                }
                return BadRequest(SmartJsonResult.Failure(chartDataAndPreferencesResult.ValidationErrors));
            }
            return BadRequest(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        }

        [HttpPost]
        [Route("OrderDateRangeList")]
        public async Task<IActionResult> OrderDateRangeList([FromBody]SmartParkListDateRangeRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = new SmartParkListDateRangeRequestViewModel
                {
                    DateFrom = DateTime.Today.AddDays(-6),
                    DateTo = DateTime.Now
                };
            }

            var dateFrom = model.DateFrom;
            var dateTo = model.DateTo;

            // ReSharper disable once PossibleInvalidOperationException
            var userId = CurrentUser.UserId.Value;

            var serviceResult = await _orderService.GetAllAsync(x => x.Date >= dateFrom && x.Date <= dateTo && x.UserId == userId);
            if (serviceResult.IsValid)
            {
                return Ok(SmartJsonResult<SmartParkListWithDateRangeViewModel<ShopOrderItemViewModel>>
                    .Success(new SmartParkListWithDateRangeViewModel<ShopOrderItemViewModel>
                    {
                        ListItems = serviceResult.Result.OrderByDescending(x => x.Date).Select(_mapper.Map<ShopOrderItemViewModel>),
                        DateTo = model.DateTo,
                        DateFrom = model.DateFrom
                    }));
            }
            return BadRequest(SmartJsonResult<SmartParkListWithDateRangeViewModel<ShopOrderItemViewModel>>.Failure(serviceResult.ValidationErrors));
        }

        [HttpPost]
        [Route("GtDateRangeList")]
        public async Task<IActionResult> GtDateRangeList([FromBody]SmartParkListDateRangeRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = new SmartParkListDateRangeRequestViewModel
                {
                    DateFrom = DateTime.Today.AddDays(-6),
                    DateTo = DateTime.Now
                };
            }

            var dateFrom = model.DateFrom;
            var dateTo = model.DateTo;

            // ReSharper disable once PossibleInvalidOperationException
            var userId = CurrentUser.UserId.Value;

            var serviceResult = await _gateUsageService.GetAllAsync(x => x.DateOfUse >= dateFrom && x.DateOfUse <= dateTo && x.UserId == userId);

            if (serviceResult.IsValid)
            {
                return Ok(SmartJsonResult<SmartParkListWithDateRangeViewModel<GateOpeningViewModel>>
                    .Success(new SmartParkListWithDateRangeViewModel<GateOpeningViewModel>
                    {
                        ListItems = serviceResult.Result.OrderByDescending(x => x.DateOfUse).Select(_mapper.Map<GateOpeningViewModel>),
                        DateTo = model.DateTo,
                        DateFrom = model.DateFrom
                    }));
            }
            return BadRequest(SmartJsonResult<SmartParkListWithDateRangeViewModel<GateOpeningViewModel>>.Failure(serviceResult.ValidationErrors));
        }
    }
}
