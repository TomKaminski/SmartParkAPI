using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.DTO.GateUsage;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Controllers.Admin.Base;
using SmartParkAPI.Infrastructure.Attributes;
using SmartParkAPI.Models.Admin.GateUsage;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Controllers.Admin
{
    public class AdminGateUsageController : AdminServiceBaseController<AdminGateUsageListItemViewModel, GateUsageBaseDto, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IGateUsageService _service;

        public AdminGateUsageController(IGateUsageService entityService, IMapper mapper) : base(entityService, mapper)
        {
            _mapper = mapper;
            _service = entityService;
        }

        public override async Task<SmartJsonResult<IEnumerable<AdminGateUsageListItemViewModel>>> ListAsync()
        {
            var serviceResult = await _service.GetAllAdminAsync();
            return serviceResult.IsValid
                ? SmartJsonResult<IEnumerable<AdminGateUsageListItemViewModel>>.Success(serviceResult.Result.Select(_mapper.Map<AdminGateUsageListItemViewModel>))
                : SmartJsonResult<IEnumerable<AdminGateUsageListItemViewModel>>.Failure(serviceResult.ValidationErrors);
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

            var serviceResult = await _service.GetAllAdminAsync(x => x.DateOfUse >= dateFrom && x.DateOfUse <= dateTo);
            return Json(serviceResult.IsValid
                ? SmartJsonResult<SmartParkListWithDateRangeViewModel<AdminGateUsageListItemViewModel>>
                .Success(new SmartParkListWithDateRangeViewModel<AdminGateUsageListItemViewModel>
                {
                    ListItems = serviceResult.Result.Select(_mapper.Map< AdminGateUsageListItemViewModel>),
                    DateTo = model.DateTo,
                    DateFrom = model.DateFrom
                })
                : SmartJsonResult<SmartParkListWithDateRangeViewModel<AdminGateUsageListItemViewModel>>.Failure(serviceResult.ValidationErrors));
        }
    }
}
