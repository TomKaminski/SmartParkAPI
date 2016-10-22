using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.DTO.Order;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Controllers.Admin.Base;
using SmartParkAPI.Models.Admin.Order;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Controllers.Admin
{
    public class AdminOrderController : AdminServiceBaseController<AdminOrderListItemViewModel, OrderBaseDto, long>
    {
        private readonly IOrderService _entityService;
        private readonly IMapper _mapper;

        public AdminOrderController(IOrderService entityService, IMapper mapper) : base(entityService, mapper)
        {
            _entityService = entityService;
            _mapper = mapper;
        }

        public override async Task<SmartJsonResult<IEnumerable<AdminOrderListItemViewModel>>> ListAsync()
        {
            var serviceResult = await _entityService.GetAllAdminAsync();
            return serviceResult.IsValid
                ? SmartJsonResult<IEnumerable<AdminOrderListItemViewModel>>.Success(serviceResult.Result.Select(_mapper.Map<AdminOrderListItemViewModel>))
                : SmartJsonResult<IEnumerable<AdminOrderListItemViewModel>>.Failure(serviceResult.ValidationErrors);
        }

        [HttpPost]
        public async Task<IActionResult> DateRangeList([FromBody]SmartParkListDateRangeRequestViewModel model)
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

            var serviceResult = await _entityService.GetAllAdminAsync(x => x.Date >= dateFrom && x.Date <= dateTo);
            return Json(serviceResult.IsValid
                ? SmartJsonResult<SmartParkListWithDateRangeViewModel<AdminOrderListItemViewModel>>
                .Success(new SmartParkListWithDateRangeViewModel<AdminOrderListItemViewModel>
                {
                    ListItems = serviceResult.Result.Select(_mapper.Map<AdminOrderListItemViewModel>),
                    DateTo = model.DateTo,
                    DateFrom = model.DateFrom
                })
                : SmartJsonResult<SmartParkListWithDateRangeViewModel<AdminOrderListItemViewModel>>.Failure(serviceResult.ValidationErrors));
        }
    }
}
