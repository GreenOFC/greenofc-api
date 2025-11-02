using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Shinhan;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class LeadShinhanProfile: Profile
    {
        public LeadShinhanProfile()
        {
            CreateMap<ShinhanPersonalDto, Personal>().ReverseMap();
            CreateMap<ShinhanWorkingDto, Working>().ReverseMap();
            CreateMap<ShinhanReferenceDto, Referee>().ReverseMap();
            CreateMap<ShinhanLoanDto, Loan>().ReverseMap();
            CreateMap<ShinhanAddressDto, Address>().ReverseMap();
            CreateMap<Models.Result, ShinhanResultDto>();
            CreateMap<Sale, ShinhanSaleDto>();
            CreateMap<CreateShinhanRequest, Customer>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => new Models.Result()))
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenE))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<CreateShinhanStep1Request, Customer>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => new Models.Result()))
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenE))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<Customer, GetShinhanDetailResponse>();
            CreateMap<UpdateShinhanRequest, Customer>();
            CreateMap<UpdateShinhanStep1Request, Customer>();
            CreateMap<UpdateShinhanStep2Request, Customer>();
            CreateMap<UpdateShinhanStep3Request, Customer>();
            CreateMap<UpdateShinhanStep4Request, Customer>();
            CreateMap<ShinhanGroupDocumentDto, GroupDocument>().ReverseMap();
            CreateMap<ShinhanDocumentUploadDto, DocumentUpload>().ReverseMap();
            CreateMap<ShinhanUploadedMediaDto, UploadedMedia>().ReverseMap();

            CreateMap<GroupDtoModel, GroupDocument>();
            CreateMap<DocumentDtoModel, DocumentUpload>();
        }
    }
}
