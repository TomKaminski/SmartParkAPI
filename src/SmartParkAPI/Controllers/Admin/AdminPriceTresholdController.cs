using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.DTO.PriceTreshold;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Controllers.Admin.Base;
using SmartParkAPI.Models.Admin.PriceTreshold;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Controllers.Admin
{
    public class AdminPriceTresholdController : AdminServiceController
                                                <AdminPriceTresholdListItemViewModel,
                                                 AdminPriceTresholdCreateViewModel,
                                                 AdminPriceTresholdEditViewModel,
                                                 AdminPriceTresholdDeleteViewModel,
                                                 PriceTresholdBaseDto, int>
    {
        private readonly IPriceTresholdService _entityService;
        private readonly IMapper _mapper;

        public AdminPriceTresholdController(IPriceTresholdService entityService, IMapper mapper) : base(entityService, mapper)
        {
            _entityService = entityService;
            _mapper = mapper;
        }

        public override Task<IActionResult> Edit(AdminPriceTresholdEditViewModel model)
        {
            throw new NotImplementedException();
        }

        public override async Task<SmartJsonResult<IEnumerable<AdminPriceTresholdListItemViewModel>>> ListAsync()
        {
            var serviceResult = await _entityService.GetAllAdminAsync();
            return serviceResult.IsValid
                ? SmartJsonResult<IEnumerable<AdminPriceTresholdListItemViewModel>>.Success(serviceResult.Result.Select(_mapper.Map<AdminPriceTresholdListItemViewModel>))
                : SmartJsonResult<IEnumerable<AdminPriceTresholdListItemViewModel>>.Failure(serviceResult.ValidationErrors);
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPrc([FromBody]AdminPriceTresholdDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var recoverUserResult = await _entityService.RecoverPriceTresholdAsync(model.Id);
                return Json(recoverUserResult.IsValid
                    ? SmartJsonResult.Success(recoverUserResult.SuccessNotifications.First())
                    : SmartJsonResult.Failure(recoverUserResult.ValidationErrors));
            }
            return Json(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        }

        [HttpPost]
        public override async Task<IActionResult> Create([FromBody]AdminPriceTresholdCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var serviceResult = await _entityService.CreateAsync(_mapper.Map<PriceTresholdBaseDto>(model));

                
                return Json(serviceResult.IsValid
                    ? SmartJsonResult<AdminPriceTresholdListItemViewModel, PrcAdminCreateInfo>
                    .Success(_mapper.Map<AdminPriceTresholdListItemViewModel>(serviceResult.Result), serviceResult.SecondResult, GetSuccessNotificationForCreate(serviceResult.SecondResult))
                    : SmartJsonResult.Failure(serviceResult.ValidationErrors));
            }
            return Json(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        }

        public string GetSuccessNotificationForCreate(PrcAdminCreateInfo createInfo)
        {
            if (createInfo.Recovered)
            {
                return "Podany przedział cenowy istniał już w bazie, przywróciliśmy go.";
            }
            else if (createInfo.ReplacedDefault)
            {
                return "Podany przedział cenowy został stworzony, oraz podmienił istniejący już bazowy przedział.";
            }
            else
            {
                return "Przedział cenowy został stworzony pomyślnie";
            }
        }
    }
}
