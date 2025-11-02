using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.CheckLoans;
using _24hplusdotnetcore.ModelDtos.F88;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.F88;
using AutoMapper;
using System;

namespace _24hplusdotnetcore.Mappings
{
    public class LeadF88Profile : Profile
    {
        public LeadF88Profile()
        {
            CreateMap<Customer, LeadF88>()
                .ForMember(dest => dest.Id, src => src.Ignore())
                .ForMember(dest => dest.CreatedDate, src => src.Ignore())
                .ForMember(dest => dest.ModifiedDate, src => src.Ignore())
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Personal.Name))
                .ForMember(dest => dest.Phone, src => src.MapFrom(x => x.Personal.Phone))
                .ForMember(dest => dest.LoanCategory, src => src.MapFrom(x => x.Loan.Category))
                .ForMember(dest => dest.IdCard, src => src.MapFrom(x => x.Personal.IdCard))
                .ForMember(dest => dest.Province, src => src.MapFrom(x => x.ResidentAddress.Province))
                .ForMember(dest => dest.Address, src => src.MapFrom(x => x.ResidentAddress.GetAddress()));

            CreateMap<LeadF88, LeadF88>();
            CreateMap<F88PostBackDto, F88Notification>();

            CreateMap<LeadF88, F88RestRequest>()
                .ForMember(dest => dest.TransactionID, src => src.MapFrom(x => ""))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.Phone, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Select1, src => src.MapFrom(x => x.LoanCategoryData.Value ?? x.LoanCategory))
                .ForMember(dest => dest.Select2, src => src.MapFrom(x => x.Address))
                .ForMember(dest => dest.Province, src => src.MapFrom(x => x.ProvinceData.Value ?? x.Province))
                .ForMember(dest => dest.Passport, src => src.MapFrom(x => x.IdCard))
                .ForMember(dest => dest.RequestId, src => src.MapFrom(x => $"{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(10000000, 99999999)}"));

            CreateMap<CreateLeadF88Request, LeadF88>();
            CreateMap<UpdateLeadF88Request, LeadF88>();
            CreateMap<LeadF88, GetLeadF88Response>();
            CreateMap<GetLeadF88Response, LeadF88>();

            CreateMap<F88PostBackResponse, PostBack>();
            CreateMap<PostBack, F88PostBackResponse>();
            CreateMap<GetF88NotiResponse, F88Notification>();
            CreateMap<F88Notification, GetF88NotiResponse>();

            CreateMap<CreateF88Request, Customer>()
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenF88))
                .ForMember(dest => dest.ProductLine, opt => opt.MapFrom(src => ProductLineEnum.DSA))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.SUBMIT));
            CreateMap<Customer, CreateF88Response>();
            CreateMap<UpdateF88Request, Customer>();
            CreateMap<Personal, F88PersonalDto>().ReverseMap();
            CreateMap<Customer, GetF88DetailResponse>();
            CreateMap<Loan, F88LoanDto>().ReverseMap();

            CreateMap<LeadF88, CheckLoanResponse>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => "F88"));
        }
    }
}
