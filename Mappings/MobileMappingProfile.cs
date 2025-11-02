using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class MobileMappingProfile : Profile
    {
        public MobileMappingProfile()
        {
            #region Customer
            CreateMap<Personal, PersonalResponseModel>();
            CreateMap<Loan, LoanResponseModel>();
            CreateMap<Models.Result, ResultResponseModel>();

            CreateMap<Customer, CustomerResponseModel>()
                .ForMember(dest => dest.Personal, src => src.MapFrom(x => x.Personal))
                .ForMember(dest => dest.Loan, src => src.MapFrom(x => x.Loan))
                .ForMember(dest => dest.Result, src => src.MapFrom(x => x.Result));

            
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.Personal, src => src.MapFrom(x => x.Personal))
                .ForMember(dest => dest.ResidentAddress, src => src.MapFrom(x => x.ResidentAddress))
                .ForMember(dest => dest.TemporaryAddress, src => src.MapFrom(x => x.TemporaryAddress))
                .ForMember(dest => dest.Working, src => src.MapFrom(x => x.Working))
                .ForMember(dest => dest.Spouse, src => src.MapFrom(x => x.Spouse))
                .ForMember(dest => dest.Referees, src => src.MapFrom(x => x.Referees))
                .ForMember(dest => dest.Loan, src => src.MapFrom(x => x.Loan))
                .ForMember(dest => dest.BankInfo, src => src.MapFrom(x => x.BankInfo))
                .ForMember(dest => dest.SaleInfo, src => src.MapFrom(x => x.SaleInfo))
                .ForMember(dest => dest.OtherInfo, src => src.MapFrom(x => x.OtherInfo))
                .ForMember(dest => dest.Result, src => src.MapFrom(x => x.Result));
            
            CreateMap<Personal, PersonalDto>();
            CreateMap<Address, AddressDto>();
            CreateMap<Working, WorkingDto>()
                .ForMember(dest => dest.CompanyAddress, src => src.MapFrom(x => x.CompanyAddress));;
            CreateMap<Referee, RefereeDto>();
            CreateMap<Loan, LoanDto>();
            CreateMap<BankInfo, BankInfoDto>();
            CreateMap<Sale, SaleDto>();
            CreateMap<Loan, OtherInfoDto>();
            CreateMap<OtherInfo, LoanDto>();
            CreateMap<Result, ResultDto>();
            #endregion
        }
    }
}
