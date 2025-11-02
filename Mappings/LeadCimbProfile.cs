using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos.LeadCimbs;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class LeadCimbProfile: Profile
    {
        public LeadCimbProfile()
        {
            CreateMap<CimbAddressDto, Address>().ReverseMap();
            CreateMap<CimbLoanDto, Loan>().ReverseMap();
            CreateMap<CimbGroupDocumentDto, GroupDocument>().ReverseMap();
            CreateMap<CimbDocumentUploadDto, DocumentUpload>().ReverseMap();
            CreateMap<CimbUploadedMediaDto, UploadedMedia>().ReverseMap();
            CreateMap<CimbPersonalDto, Personal>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToUpper()));
            CreateMap<Personal, CimbPersonalResponseDto>();
            CreateMap<CimbReferenceDto, Referee>().ReverseMap();
            CreateMap<CimbWorkingDto, Working>().ReverseMap();
            CreateMap<Models.Result, CimbResultDto>();
            CreateMap<CreateLeadCimbRequest, Customer>()
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenG))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<Customer, CreateLeadCimbResponse>();
            CreateMap<UpdateLeadCimbStep1Request, Customer>();
            CreateMap<UpdateLeadCimbStep2Request, Customer>();
            CreateMap<UpdateLeadCimbStep3Request, Customer>();
            CreateMap<UpdateLeadCimbStep4Request, Customer>();
            CreateMap<Customer, GetLeadCimbDetailResponse>();
            CreateMap<Customer, CimbSendVerifyResponse>();

            CreateMap<CimbResourceItemRestDto, LeadCimbResource>();
            CreateMap<LeadCimbResource, GetCimbCityResponse>()
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.StateDesc, opt => opt.MapFrom(src => src.Vi));
            CreateMap<LeadCimbResource, GetCimbDistrictResponse>()
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Vi))
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.ParentCode));
            CreateMap<LeadCimbResource, GetCimbWardResponse>()
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.ZipDesc, opt => opt.MapFrom(src => src.Vi))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.ParentCode));

            CreateMap<Customer, UpdateLeadCimbStep1Response>();
            CreateMap<Sale, CimbSaleDto>();
        }
    }
}
