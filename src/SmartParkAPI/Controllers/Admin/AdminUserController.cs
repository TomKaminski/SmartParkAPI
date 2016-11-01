using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Controllers.Admin.Base;
using SmartParkAPI.Models.Admin.User;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Controllers.Admin
{
    public class AdminUserController : AdminServiceController<AdminUserListItemViewModel, AdminUserCreateViewModel, 
        AdminUserEditViewModel, AdminUserDeleteViewModel, UserBaseDto, int>
    {
        private readonly IUserService _entityService;
        private readonly IMapper _mapper;

        public AdminUserController(IUserService entityService, IMapper mapper) : base(entityService, mapper)
        {
            _entityService = entityService;
            _mapper = mapper;
        }

        public override Task<IActionResult> Create(AdminUserCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        public override IActionResult List()
        {
            var serviceResult = _entityService.GetAllAdmin();
            if (serviceResult.IsValid)
            {
                return Ok(SmartJsonResult<IEnumerable<AdminUserListItemViewModel>>.Success(serviceResult.Result.Select(_mapper.Map<AdminUserListItemViewModel>)));
            }
            return BadRequest(SmartJsonResult<IEnumerable<AdminUserListItemViewModel>>.Failure(serviceResult.ValidationErrors));
        }

        [HttpPost]
        public async Task<IActionResult> RecoverUser([FromBody]AdminUserDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var recoverUserResult = await _entityService.RecoverUserAsync(model.Id);
                if (recoverUserResult.IsValid)
                {
                    return Ok(SmartJsonResult.Success("Operacja przywrócenia użytkownika zakończona pomyślnie."));
                }
                return BadRequest(SmartJsonResult.Failure(recoverUserResult.ValidationErrors));
            }
            return BadRequest(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        }

        [HttpPost]
        public override async Task<IActionResult> Edit([FromBody]AdminUserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var serviceResult = await _entityService.AdminEditAsync(_mapper.Map<UserBaseDto>(model), model.OldEmail);
                if (serviceResult.IsValid)
                {
                    return Ok(SmartJsonResult.Success("Edycja użytkownika zakończona pomyślnie"));
                }
                return BadRequest(SmartJsonResult.Failure(serviceResult.ValidationErrors));
            }
            return BadRequest(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        }
    }
}
