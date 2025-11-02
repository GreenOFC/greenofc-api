using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.CheckLoans;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.F88;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ICheckLoanService
    {
        Task<PagingResponse<CheckLoanResponse>> CheckLoanAsync(CheckLoanRequest checkLoanRequest);
    }

    public class CheckLoanService : ICheckLoanService, IScopedLifetime
    {
        private readonly ILogger<CheckLoanService> _logger;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILeadF88Repository _leadF88Repository;

        public CheckLoanService(
            ILogger<CheckLoanService> logger,
            IMapper mapper,
            ICustomerRepository customerRepository,
            ILeadF88Repository leadF88Repository)
        {
            _logger = logger;
            _mapper = mapper;
            _customerRepository = customerRepository;
            _leadF88Repository = leadF88Repository;
        }

        public async Task<PagingResponse<CheckLoanResponse>> CheckLoanAsync(CheckLoanRequest checkLoanRequest)
        {
            try
            {
                string[] greenTypes = { GreenType.GreenA, GreenType.GreenC, GreenType.GreenP };
                var customers = await _customerRepository.FilterByAsync(x => x.Personal.IdCard == checkLoanRequest.IdCard && greenTypes.Contains(x.GreenType));
                var f88s = await _leadF88Repository.GetByIdCardAsync(checkLoanRequest.IdCard);

                var customerDtos = _mapper.Map<IEnumerable<CheckLoanResponse>>(customers);
                var f88Dtos = _mapper.Map<IEnumerable<CheckLoanResponse>>(f88s);

                return new PagingResponse<CheckLoanResponse>
                {
                    Data = customerDtos.Concat(f88Dtos),
                    TotalRecord = customerDtos.Count() + f88Dtos.Count()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
