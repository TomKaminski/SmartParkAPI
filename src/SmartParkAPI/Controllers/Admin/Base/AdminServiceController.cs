using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.Services.Base;
using SmartParkAPI.Infrastructure.Attributes;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Controllers.Admin.Base
{
    public class AdminServiceController<TListViewModel, TCreateViewModel, TEditViewModel, TDeleteViewModel, TDto, TKeyType> 
        : AdminServiceBaseController<TListViewModel, TDto, TKeyType>
        where TListViewModel : SmartParkListBaseViewModel
        where TCreateViewModel : SmartParkCreateBaseViewModel
        where TEditViewModel : SmartParkEditBaseViewModel<TKeyType>
        where TKeyType : struct
        where TDeleteViewModel : SmartParkDeleteBaseViewModel<TKeyType>
        where TDto : BaseDto<TKeyType>
    {
        private readonly IEntityService<TDto, TKeyType> _entityService;
        private readonly IMapper _mapper;

        public AdminServiceController(IEntityService<TDto, TKeyType> entityService, IMapper mapper) : base(entityService, mapper)
        {
            _entityService = entityService;
            _mapper = mapper;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody]TCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var serviceResult = await _entityService.CreateAsync(_mapper.Map<TDto>(model));
                if (serviceResult.IsValid)
                {
                    return Ok(SmartJsonResult<TListViewModel>.Success(_mapper.Map<TListViewModel>(serviceResult.Result)));
                }
                return BadRequest(SmartJsonResult.Failure(serviceResult.ValidationErrors));
            }
            return BadRequest(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete([FromBody]TDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var serviceResult = await _entityService.DeleteAsync(model.Id);
                if (serviceResult.IsValid)
                {
                    return Ok(SmartJsonResult.Success("Operacja usunięcia zakończona pomyślnie."));
                }
                return BadRequest(SmartJsonResult.Failure(serviceResult.ValidationErrors));
            }
            return BadRequest(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Edit([FromBody]TEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var serviceResult = await _entityService.EditAsync(_mapper.Map<TDto>(model));
                if (serviceResult.IsValid)
                {
                    return Ok(SmartJsonResult<TListViewModel>.Success(_mapper.Map<TListViewModel>(serviceResult.Result), "Edycja zakończona pomyślnie"));
                }
                return BadRequest(SmartJsonResult.Failure(serviceResult.ValidationErrors));
            }
            return BadRequest(SmartJsonResult.Failure(GetModelStateErrors(ModelState)));
        }
    }
}
