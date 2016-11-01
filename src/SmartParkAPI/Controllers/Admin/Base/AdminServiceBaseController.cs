using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Controllers.Admin.Base
{
    public class AdminServiceBaseController<TListViewModel, TDto, TKeyType> : AdminBaseController
        where TListViewModel : SmartParkListBaseViewModel
        where TKeyType : struct
        where TDto : BaseDto<TKeyType>
    {
        private readonly IEntityService<TDto, TKeyType> _entityService;
        private readonly IMapper _mapper;

        public AdminServiceBaseController(IEntityService<TDto, TKeyType> entityService, IMapper mapper)
        {
            _entityService = entityService;
            _mapper = mapper;
        }

        public virtual async Task<IActionResult> ListAsync()
        {
            var serviceResult = await GetAllAsync();
            if (serviceResult.IsValid)
            {
                return Ok(SmartJsonResult<IEnumerable<TListViewModel>>.Success(serviceResult.Result.Select(_mapper.Map<TListViewModel>)));
            }
            return BadRequest(SmartJsonResult<IEnumerable<TListViewModel>>.Failure(serviceResult.ValidationErrors));
        }

        public virtual IActionResult List()
        {
            var serviceResult = _entityService.GetAll();
            if (serviceResult.IsValid)
            {
                return Ok(SmartJsonResult<IEnumerable<TListViewModel>>.Success(serviceResult.Result.Select(_mapper.Map<TListViewModel>)));
            }
            return BadRequest(SmartJsonResult<IEnumerable<TListViewModel>>.Failure(serviceResult.ValidationErrors));
        }

        protected virtual async Task<ServiceResult<IEnumerable<TDto>>> GetAllAsync()
        {
            return await _entityService.GetAllAsync();
        }
    }
}
