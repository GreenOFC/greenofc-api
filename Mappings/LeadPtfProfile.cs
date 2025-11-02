using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.LeadPtf;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class LeadPtfProfile: Profile
    {
        public LeadPtfProfile()
        {
            CreateMap<KeyValueDto, KeyValueModel>().ReverseMap();
            CreateMap<Personal, LeadPtfPersonalDto>();
            CreateMap<LeadPtfWorkingDto, Working>().ReverseMap();
            CreateMap<LeadPtfAddressDto, Address>().ReverseMap();
            CreateMap<LeadPtfShortAddressDto, Address>().ReverseMap();
            CreateMap<LeadPtfRefereeDto, Referee>().ReverseMap();
            CreateMap<LeadPtfLoanDto, Loan>().ReverseMap();
            CreateMap<LeadPtfResultDto, Result>().ReverseMap();
            CreateMap<LeadPtfDisbursementInformationDto, DisbursementInformation>().ReverseMap();
            CreateMap<LeadPtfGroupDocumentDto, GroupDocument>().ReverseMap();
            CreateMap<LeadPtfDocumentUploadDto, DocumentUpload>().ReverseMap();
            CreateMap<LeadPtfUploadedMediaDto, UploadedMedia>().ReverseMap();
            CreateMap<CreateLeadPtfRequest, Customer>()
                .ForMember(dest => dest.Personal, opt => opt.MapFrom(src => src.Personal))
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenP))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<Customer, CreateLeadPtfResponse>();
            CreateMap<UpdateLeadPtfRequest, Customer>()
                .ForMember(dest => dest.Personal, opt => opt.MapFrom(src => src.Personal));
            CreateMap<UpdateDocumentLeadPtfRequest, Customer>();
            CreateMap<Customer, GetDetailLeadPtfResponse>();
            CreateMap<LeadPtfPersonalDto, Personal>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToUpper()));
            

            CreateMap<LeadPtfCategoryGroup, LeadPtfCategoryGroup>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<LeadPtfCategory, LeadPtfCategory>();
            CreateMap<LeadPtfProduct, LeadPtfProduct>();
            CreateMap<ChecklistModel, ChecklistModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<LeadPtfProduct, LeadPtfProductDto>();
            CreateMap<LeadPtfCategory, LeadPtfCategoryDto>();
            CreateMap<LeadPtfCategoryGroup, LeadPtfCategoryGroupDto>();

            CreateMap<CreateLeadPtfStep1Request, Customer>()
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenP))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<UpdateLeadPtfStep1Request, Customer>();
            CreateMap<UpdateLeadPtfStep2Request, Customer>();
            CreateMap<UpdateLeadPtfStep3Request, Customer>();
            CreateMap<UpdateLeadPtfStep4Request, Customer>();
            CreateMap<UpdateLeadPtfStep5Request, Customer>();
        }
    }
}
