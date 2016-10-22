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

        //[Route("~/[area]/[controller]")]
        //public virtual IActionResult Index()
        //{
        //    return PartialView();
        //}

        public virtual async Task<SmartJsonResult<IEnumerable<TListViewModel>>> ListAsync()
        {
            var serviceResult = await GetAllAsync();
            return serviceResult.IsValid 
                ? SmartJsonResult<IEnumerable<TListViewModel>>.Success(serviceResult.Result.Select(_mapper.Map<TListViewModel>)) 
                : SmartJsonResult<IEnumerable<TListViewModel>>.Failure(serviceResult.ValidationErrors);
        }

        public virtual SmartJsonResult<IEnumerable<TListViewModel>> List()
        {
            var serviceResult = _entityService.GetAll();
            return serviceResult.IsValid
                ? SmartJsonResult<IEnumerable<TListViewModel>>.Success(serviceResult.Result.Select(_mapper.Map<TListViewModel>))
                : SmartJsonResult<IEnumerable<TListViewModel>>.Failure(serviceResult.ValidationErrors);
        }

        protected virtual async Task<ServiceResult<IEnumerable<TDto>>> GetAllAsync()
        {
            return await _entityService.GetAllAsync();
        }
    }
}
