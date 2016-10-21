using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParkAPI.Business.Services.Base;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.GateUsage;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.Model.Concrete;

namespace SmartParkAPI.Business.Services
{
    public class GateUsageService : EntityService<GateUsageBaseDto, GateUsage, Guid>, IGateUsageService
    {
        private readonly IGateUsageRepository _repository;
        private readonly IMapper _mapper;

        public GateUsageService(IUnitOfWork unitOfWork, IGateUsageRepository repository, IMapper mapper)
            : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IEnumerable<GateUsageAdminDto>>> GetAllAdminAsync()
        {
            return ServiceResult<IEnumerable<GateUsageAdminDto>>
                .Success((await _repository.Include(x => x.User).ToListAsync())
                .Select(_mapper.Map<GateUsageAdminDto>));
        }

        public async Task<ServiceResult<IEnumerable<GateUsageAdminDto>>> GetAllAdminAsync(Expression<Func<GateUsageBaseDto, bool>> predicate)
        {
            return ServiceResult<IEnumerable<GateUsageAdminDto>>
                .Success((await _repository.Include(x => x.User)
                .Where(MapExpressionToEntity(predicate))
                .ToListAsync())
                .Select(_mapper.Map<GateUsageAdminDto>));
        }
    }
}
