using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MCDebts;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.MC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MC
{
    public interface IMCDebtService
    {
        Task CreateAsync(int appNumber);
        Task UpdateAsync(int appNumber);
        Task<PagingResponse<GetMCDebtResponse>> GetAsync(GetMCDebtRequest getMCDebtRequest);
        Task ConfirmPaymentAsync(string id);
        Task UnFollowAsync(string id);
        Task<GetDetailMCDebtResponse> GetDetailAsync(string id);
    }
    public class MCDebtService : IMCDebtService, IScopedLifetime
    {
        private readonly ILogger<MCDebtService> _logger;
        private readonly IMCDebtRepository _mCDebtRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IRestMCService _restMCService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ProductServices _productServices;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;

        public MCDebtService(
            IMCDebtRepository mCDebtRepository,
            ILogger<MCDebtService> logger,
            IUserLoginService userLoginService,
            IRestMCService restMCService,
            ICustomerRepository customerRepository,
            IUserRepository userRepository,
            ProductServices productServices,
            IMapper mapper,
            IUserServices userServices)
        {
            _mCDebtRepository = mCDebtRepository;
            _logger = logger;
            _userLoginService = userLoginService;
            _restMCService = restMCService;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _productServices = productServices;
            _mapper = mapper;
            _userServices = userServices;
        }

        public async Task CreateAsync(int appNumber)
        {
            try
            {
                var approvalInfo = await _restMCService.GetApprovedInfoAsync(appNumber);
                if (approvalInfo == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, "Thông tin khoản vay"));
                }
                var mCDebt = _mapper.Map<MCDebt>(approvalInfo);
                var customer = await _customerRepository.FindOneAsync(x => x.MCAppnumber == appNumber);
                var user = await _userRepository.FindOneAsync(x => x.UserName == customer.SaleInfo.Code);

                if(decimal.TryParse(mCDebt.LoanApprovedAmt, out decimal totalLoanAmt))
                {
                    customer.Result.ApprovedAmount = totalLoanAmt;
                }
                customer.Result.ContractNumber = mCDebt.ContractNumber;
                customer.Result.ApprovedDate = DateTime.Now;
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);
                // var product = _productServices.GetProductByProductId(customer.Loan.ProductId);
                // if (product != null)
                // {
                //     double.TryParse(mCDebt.LoanApprovedAmt, out double loanAmount);
                //     int.TryParse(mCDebt.LoanApprovedTenor, out int term);
                //     double.TryParse(product.InterestRateByMonth, out double rate);
                //     if (mCDebt.HasInsurrance)
                //     {
                //         loanAmount += loanAmount * 0.055;
                //     }
                //     double totalLoanAmt = loanAmount + loanAmount * rate * term / 100;
                //     mCDebt.TotalLoanAmt = totalLoanAmt.ToString();
                //     mCDebt.MonthlyPayment = (totalLoanAmt / term).ToString();
                // }
                mCDebt.Creator = user != null ? user.Id : "";
                mCDebt.LoanCategory = customer.Loan.Category;
                mCDebt.LoanProduct = customer.Loan.Product;
                mCDebt.Phone = customer.Personal.Phone;
                mCDebt.ContractCode = customer.ContractCode;


                var existedDebt = await _mCDebtRepository.FindOneAsync(x => x.AppNumber.Equals(appNumber.ToString()));
                if (existedDebt != null)
                {
                    mCDebt.Id = existedDebt.Id;
                    await _mCDebtRepository.ReplaceOneAsync(mCDebt);
                }
                else
                {
                    await _mCDebtRepository.InsertOneAsync(mCDebt);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task UpdateAsync(int appNumber)
        {
            try
            {
                var existedDebt = await _mCDebtRepository.FindOneAsync(x => x.AppNumber.Equals(appNumber.ToString()));
                if (existedDebt != null)
                {
                    existedDebt.DisbursementDate = DateTime.Now;
                    existedDebt.NextPaymentDate = DateTime.Now.AddMonths(1);
                    await _mCDebtRepository.ReplaceOneAsync(existedDebt);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<PagingResponse<GetMCDebtResponse>> GetAsync(GetMCDebtRequest getMCDebtRequest)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadMcDebtManagement_ViewAll, 
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadMcDebtManagement_ViewAll, 
                    PermissionCost.PosLeadPermission.PosLead_LeadMcDebtManagement_ViewAll, 
                    PermissionCost.AsmPermission.Asm_LeadMcDebtManagement_ViewAll, 
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadMcDebtManagement_ViewAll);

                var mcDebts = await _mCDebtRepository.GetAsync(getMCDebtRequest.Status, getMCDebtRequest.TextSearch, filterByCreatorIds, getMCDebtRequest.PageIndex, getMCDebtRequest.PageSize);

                var total = await _mCDebtRepository.CountAsync(getMCDebtRequest.Status, getMCDebtRequest.TextSearch, filterByCreatorIds);

                var result = new PagingResponse<GetMCDebtResponse>
                {
                    TotalRecord = total,
                    Data = mcDebts
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ConfirmPaymentAsync(string id)
        {
            try
            {
                var mcDebt = await _mCDebtRepository.FindByIdAsync(id);
                if (mcDebt == null)
                {
                    throw new ArgumentException(Common.Message.MC_DEBT_NOT_FOUND);
                }
                mcDebt.CurrentDebtPeriod++;
                mcDebt.NextPaymentDate = mcDebt.DisbursementDate.AddMonths(mcDebt.CurrentDebtPeriod + 1);
                mcDebt.Modifier = _userLoginService.GetUserId();
                mcDebt.ModifiedDate = DateTime.Now;

                await _mCDebtRepository.ReplaceOneAsync(mcDebt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UnFollowAsync(string id)
        {
            try
            {
                var mcDebt = await _mCDebtRepository.FindByIdAsync(id);
                if (mcDebt == null)
                {
                    throw new ArgumentException(Common.Message.MC_DEBT_NOT_FOUND);
                }
                mcDebt.IsFollowed = !mcDebt.IsFollowed;
                mcDebt.Modifier = _userLoginService.GetUserId();
                mcDebt.ModifiedDate = DateTime.Now;

                await _mCDebtRepository.ReplaceOneAsync(mcDebt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetDetailMCDebtResponse> GetDetailAsync(string id)
        {
            try
            {
                var mcDebt = await _mCDebtRepository.GetDetailAsync(id);
                if (mcDebt == null)
                {
                    throw new ArgumentException(Common.Message.MC_DEBT_NOT_FOUND);
                }
                return mcDebt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
