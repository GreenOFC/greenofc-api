using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos.LeadEcs;
using _24hplusdotnetcore.ModelResponses.EC;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class LeadEcProfile: Profile
    {
        public LeadEcProfile()
        {
            CreateMap<LeadEcPersonalDto, Personal>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToUpper()))
               .ReverseMap();
            CreateMap<CreateLeadEcRequest, Customer>()
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenD))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<Customer, CreateLeadEcResponse>();
            CreateMap<LeadEcAddressDto, Address>().ReverseMap();
            CreateMap<UpdateLeadEcLoanStep3Request, Loan>();
            CreateMap<UpdateLeadEcLoanStep4Request, Loan>();
            CreateMap<UpdateLeadEcWorkingStep3Request, Working>();
            CreateMap<UpdateLeadEcWorkingStep4Request, Working>();
            CreateMap<LeadEcReferenceDto, Referee>().ReverseMap();
            CreateMap<UpdateLeadEcWorkingStep5Request, Working>();
            CreateMap<LeadEcDisbursementInformationDto, DisbursementInformation>().ReverseMap();
            CreateMap<Loan, GetLeadEcLoanResponse>();
            CreateMap<Working, GetLeadEcWorkingResponse>();
            CreateMap<LeadEcGroupDocumentDto, GroupDocument>().ReverseMap();
            CreateMap<LeadEcDocumentUploadDto, DocumentUpload>().ReverseMap();
            CreateMap<LeadEcUploadedMediaDto, UploadedMedia>().ReverseMap();
            CreateMap<LeadEcSelectedOfferDto, LeadEcSelectedOffer>().ReverseMap();
            CreateMap<LeadEcResultDto, Result>().ReverseMap();

            CreateMap<UpdateLeadEcStep1Request, Customer>();
            CreateMap<UpdateLeadEcStep2Request, Customer>();
            CreateMap<UpdateLeadEcStep3Request, Customer>();
            CreateMap<UpdateLeadEcStep4Request, Customer>();
            CreateMap<UpdateLeadEcStep5Request, Customer>();
            CreateMap<UpdateLeadEcStep6Request, Customer>();
            CreateMap<UpdateLeadEcStep7Request, Customer>();
            CreateMap<UpdateLeadEcStep8Request, Customer>();
            CreateMap<UpdateEcRecordFileRequest, Customer>();
            CreateMap<Customer, GetLeadEcDetailResponse>();
            CreateMap<LeadEcResource, GetLeadEcCityResponse>()
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.StateDesc, opt => opt.MapFrom(src => src.Vi));
            CreateMap<LeadEcResource, GetLeadEcDistrictResponse>()
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Vi))
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.ParentCode));
            CreateMap<LeadEcResource, GetLeadEcWardResponse>()
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.ZipDesc, opt => opt.MapFrom(src => src.Vi))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.ParentCode));

            CreateMap<LeadEcResource, GetLeadEcBankBranchResponse>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Vi));
            CreateMap<LeadEcResource, GetLeadEcBankResponse>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Vi));

            CreateMap<LeadEcProductItem, ProductDocumentDto>();

            CreateMap<LeadEcProductItem, Customer>();
            CreateMap<LeadEcDocument, GroupDocument>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.BundleCode))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.BundleName))
                .ForMember(dest => dest.Mandatory, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.DocumentItems));
            CreateMap<LeadEcDocumentItem, DocumentUpload>()
                .ForMember(dest => dest.DocumentCode, opt => opt.MapFrom(src => src.DocType))
                .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src => src.DocDescriptionVi));
            CreateMap<Sale, LeadEcSaleDto>();

            CreateMap<ECProductListDataResponse, LeadEcProduct>();
            CreateMap<ECParentDocumentCollectingResponse, LeadEcProductItem>();
            CreateMap<ECChildDocumentCollectingResponse, LeadEcDocument>();
            CreateMap<ECDocumentList, LeadEcDocumentItem>();
        }
    }
}
