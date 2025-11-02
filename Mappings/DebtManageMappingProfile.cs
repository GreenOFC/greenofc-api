using _24hplusdotnetcore.ModelDtos.DebtManagement;
using _24hplusdotnetcore.ModelResponses.DebtManagement;
using _24hplusdotnetcore.Models.DebtManagement;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class DebtManageMappingProfile : Profile
    {
        public DebtManageMappingProfile()
        {
            CreateMap<DebtManagementModel, DebtDetailResponse>().ReverseMap();
            CreateMap<DebtManagementModel, CreateDebtDto>().ReverseMap();
            CreateMap<DebtManagementModel, UpdateDebtDto>().ReverseMap();
            CreateMap<DebtLoan, DebtLoanDto>().ReverseMap();
            CreateMap<DebtPersonal, DebtPersonalDto>().ReverseMap();
            CreateMap<DebtSaleInfo, DebtSaleInfoDto>().ReverseMap();
            CreateMap<DebtLoan, DebtLoanResponse>().ReverseMap();
            CreateMap<DebtPersonal, DebtPersonalResponse>().ReverseMap();
            CreateMap<DebtSaleInfo, DebtSaleInfoResponse>().ReverseMap();
            CreateMap<ImportDebtDto, ImportDebtDetailResponse>().ReverseMap();
            CreateMap<ImportOverDueDateDto, ImportDebtDetailResponse>().ReverseMap();
            CreateMap<DebtTeamleadInfo, DebtTeamLeadResponse>().ReverseMap();
            CreateMap<DebtPosInfo, DebtPosResponse>().ReverseMap();

        }
    }
}