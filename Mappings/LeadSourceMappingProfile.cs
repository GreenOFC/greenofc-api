using _24hplusdotnetcore.ModelDtos.Lead;
using _24hplusdotnetcore.ModelDtos.LeadHomes;
using _24hplusdotnetcore.ModelDtos.LeadOkVays;
using _24hplusdotnetcore.ModelDtos.LeadVbis;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.F88;
using _24hplusdotnetcore.Models.Lead;
using _24hplusdotnetcore.Models.VPS;
using AutoMapper;
using System.Reflection;

namespace _24hplusdotnetcore.Mappings
{
    public class LeadSourceMappingProfile: Profile
    {
        public LeadSourceMappingProfile()
        {
            CreateMap<LeadVps, CRMRequestDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => string.IsNullOrEmpty(x.CRMId) ? null : x.CRMId))
                .ForMember(dest => dest.Leadsource, src => src.MapFrom(x => "VPS"))
                .ForMember(dest => dest.Potentialname, src => src.MapFrom(x => x.FullName))
                .ForMember(dest => dest.Cf1050, src => src.MapFrom(x => x.IdCard))
                .ForMember(dest => dest.Cf854, src => src.MapFrom(x => x.PhoneNumber))
                .ForMember(dest => dest.AssignedUserId, src => src.MapFrom(x => "19x3017"))
                .ForMember(dest => dest.Cf1514, src => src.MapFrom(x => x.PosInfo.Name))
                .ForMember(dest => dest.Cf1516, src => src.MapFrom(x => x.PosInfo.Manager.Name))
                .ForMember(dest => dest.Cf1518, src => src.MapFrom(x => x.TeamLeadInfo.FullName))
                .ForMember(dest => dest.Cf1414, src => src.MapFrom(x => x.SaleInfomation.FullName))
                .ForMember(dest => dest.Cf1422, src => src.MapFrom(x => x.SaleInfomation.UserName))
                .ForAllOtherMembers(o =>
                {
                    if (((PropertyInfo)o.DestinationMember).PropertyType == typeof(string))
                    {
                        o.MapFrom(s => "-");
                    }
                });

            CreateMap<LeadVib, CRMRequestDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => string.IsNullOrEmpty(x.CRMId) ? null : x.CRMId))
                .ForMember(dest => dest.Leadsource, src => src.MapFrom(x => "VIB"))
                .ForMember(dest => dest.Potentialname, src => src.MapFrom(x => x.FullName))
                .ForMember(dest => dest.Cf1050, src => src.MapFrom(x => string.IsNullOrEmpty(x.IdCard) ? "-" : x.IdCard))
                .ForMember(dest => dest.Cf1026, src => src.MapFrom(x => string.IsNullOrEmpty(x.Gender) ? "-" : x.Gender))
                .ForMember(dest => dest.Cf948, src => src.MapFrom(x => $"{x.GetDateOfBirth():yyyy-MM-dd}"))
                .ForMember(dest => dest.Cf854, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Cf1242, src => src.MapFrom(x => x.TemporaryAddress.GetFullAddress()))
                .ForMember(dest => dest.Cf884, src => src.MapFrom(x => string.IsNullOrEmpty(x.Income.Value) ? "-" : x.Income.Value))
                .ForMember(dest => dest.Cf1040, src => src.MapFrom(x => x.Product.Value))
                .ForMember(dest => dest.Cf1468, src => src.MapFrom(x => x.Constitution.Value))
                .ForMember(dest => dest.AssignedUserId, src => src.MapFrom(x => "19x3017"))
                .ForMember(dest => dest.Cf1514, src => src.MapFrom(x => x.PosInfo.Name))
                .ForMember(dest => dest.Cf1516, src => src.MapFrom(x => x.PosInfo.Manager.Name))
                .ForMember(dest => dest.Cf1518, src => src.MapFrom(x => x.TeamLeadInfo.FullName))
                .ForMember(dest => dest.Cf1414, src => src.MapFrom(x => x.SaleInfomation.FullName))
                .ForMember(dest => dest.Cf1422, src => src.MapFrom(x => x.SaleInfomation.UserName))
                .ForAllOtherMembers(o =>
                {
                    if (((PropertyInfo)o.DestinationMember).PropertyType == typeof(string))
                    {
                        o.MapFrom(s => "-");
                    }
                });

            CreateMap<LeadF88, CRMRequestDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => string.IsNullOrEmpty(x.CRMId) ? null : x.CRMId))
                .ForMember(dest => dest.Leadsource, src => src.MapFrom(x => "MobileGreenF"))
                .ForMember(dest => dest.Potentialname, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.Cf1050, src => src.MapFrom(x => string.IsNullOrEmpty(x.IdCard) ? "-" : x.IdCard))
                .ForMember(dest => dest.Cf1408, src => src.MapFrom(x => x.IdCardProvince))
                .ForMember(dest => dest.Cf1026, src => src.MapFrom(x => string.IsNullOrEmpty(x.Gender) ? "-" : x.Gender))
                .ForMember(dest => dest.Cf948, src => src.MapFrom(x => $"{x.GetDateOfBirth():yyyy-MM-dd}"))
                .ForMember(dest => dest.Cf854, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Cf1002, src => src.MapFrom(x => x.GetFullAddress()))
                .ForMember(dest => dest.Cf1210, src => src.MapFrom(x => x.LoanCategoryData.Value ?? x.LoanCategory))
                .ForMember(dest => dest.Cf1054, src => src.MapFrom(x => x.SignAddressData.Value ?? x.SignAddress))
                .ForMember(dest => dest.Cf1430, src => src.MapFrom(x => x.Status))
                .ForMember(dest => dest.AssignedUserId, src => src.MapFrom(x => "19x3017"))
                .ForMember(dest => dest.Cf1514, src => src.MapFrom(x => x.PosInfo.Name))
                .ForMember(dest => dest.Cf1516, src => src.MapFrom(x => x.PosInfo.Manager.Name))
                .ForMember(dest => dest.Cf1518, src => src.MapFrom(x => x.TeamLeadInfo.FullName))
                .ForMember(dest => dest.Cf1414, src => src.MapFrom(x => x.SaleInfomation.FullName))
                .ForMember(dest => dest.Cf1422, src => src.MapFrom(x => x.SaleInfomation.UserName))
                .ForAllOtherMembers(o =>
                {
                    if (((PropertyInfo)o.DestinationMember).PropertyType == typeof(string))
                    {
                        o.MapFrom(s => "-");
                    }
                });

            CreateMap<LeadHome, CRMRequestDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => string.IsNullOrEmpty(x.CRMId) ? null : x.CRMId))
                .ForMember(dest => dest.Leadsource, src => src.MapFrom(x => "HOMECREDIT"))
                .ForMember(dest => dest.Potentialname, src => src.MapFrom(x => x.FullName))
                .ForMember(dest => dest.Cf1050, src => src.MapFrom(x => string.IsNullOrEmpty(x.IdCard) ? null : x.IdCard))
                .ForMember(dest => dest.Cf854, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Cf1242, src => src.MapFrom(x => x.TemporaryAddress.GetFullAddress()))
                .ForMember(dest => dest.AssignedUserId, src => src.MapFrom(x => "19x3017"))
                .ForMember(dest => dest.Cf1514, src => src.MapFrom(x => x.PosInfo.Name))
                .ForMember(dest => dest.Cf1516, src => src.MapFrom(x => x.PosInfo.Manager.Name))
                .ForMember(dest => dest.Cf1518, src => src.MapFrom(x => x.TeamLeadInfo.FullName))
                .ForMember(dest => dest.Cf1414, src => src.MapFrom(x => x.SaleInfomation.FullName))
                .ForMember(dest => dest.Cf1422, src => src.MapFrom(x => x.SaleInfomation.UserName))
                .ForAllOtherMembers(o =>
                {
                    if (((PropertyInfo)o.DestinationMember).PropertyType == typeof(string))
                    {
                        o.MapFrom(s => "-");
                    }
                });

            CreateMap<LeadOkVay, CRMRequestDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => string.IsNullOrEmpty(x.CRMId) ? null : x.CRMId))
                .ForMember(dest => dest.Leadsource, src => src.MapFrom(x => "OK VAY"))
                .ForMember(dest => dest.Potentialname, src => src.MapFrom(x => x.FullName))
                .ForMember(dest => dest.Cf1050, src => src.MapFrom(x => string.IsNullOrEmpty(x.IdCard) ? null : x.IdCard))
                .ForMember(dest => dest.Cf854, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Cf1242, src => src.MapFrom(x => x.TemporaryAddress.GetFullAddress()))
                .ForMember(dest => dest.AssignedUserId, src => src.MapFrom(x => "19x3017"))
                .ForMember(dest => dest.Cf884, src => src.MapFrom(x => string.IsNullOrEmpty(x.Income) ? "-" : x.Income))
                .ForMember(dest => dest.Cf1514, src => src.MapFrom(x => x.PosInfo.Name))
                .ForMember(dest => dest.Cf1516, src => src.MapFrom(x => x.PosInfo.Manager.Name))
                .ForMember(dest => dest.Cf1518, src => src.MapFrom(x => x.TeamLeadInfo.FullName))
                .ForMember(dest => dest.Cf1414, src => src.MapFrom(x => x.SaleInfomation.FullName))
                .ForMember(dest => dest.Cf1422, src => src.MapFrom(x => x.SaleInfomation.UserName))
                .ForMember(dest => dest.Cf1522, src => src.MapFrom(x => x.IncomeId == "Yes" ? "1" : "0" ))
                .ForMember(dest => dest.Cf1524, src => src.MapFrom(x => x.DebtId == "Yes" ? "1" : "0" ))
                .ForMember(dest => dest.Cf1526, src => src.MapFrom(x => x.ExtraPhone))
                .ForAllOtherMembers(o =>
                {
                    if (((PropertyInfo)o.DestinationMember).PropertyType == typeof(string))
                    {
                        o.MapFrom(s => "-");
                    }
                });

            CreateMap<LeadVbi, CRMRequestDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => string.IsNullOrEmpty(x.CRMId) ? null : x.CRMId))
                .ForMember(dest => dest.Leadsource, src => src.MapFrom(x => "VBI"))
                .ForMember(dest => dest.Potentialname, src => src.MapFrom(x => x.FullName))
                .ForMember(dest => dest.Cf1050, src => src.MapFrom(x => string.IsNullOrEmpty(x.IdCard) ? null : x.IdCard))
                .ForMember(dest => dest.Cf854, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Cf1242, src => src.MapFrom(x => x.TemporaryAddress.GetFullAddress()))
                .ForMember(dest => dest.AssignedUserId, src => src.MapFrom(x => "19x3017"))
                .ForMember(dest => dest.Cf1514, src => src.MapFrom(x => x.PosInfo.Name))
                .ForMember(dest => dest.Cf1516, src => src.MapFrom(x => x.PosInfo.Manager.Name))
                .ForMember(dest => dest.Cf1518, src => src.MapFrom(x => x.TeamLeadInfo.FullName))
                .ForMember(dest => dest.Cf1414, src => src.MapFrom(x => x.SaleInfomation.FullName))
                .ForMember(dest => dest.Cf1422, src => src.MapFrom(x => x.SaleInfomation.UserName))
                .ForMember(dest => dest.Cf1526, src => src.MapFrom(x => x.ExtraPhone))
                .ForAllOtherMembers(o =>
                {
                    if (((PropertyInfo)o.DestinationMember).PropertyType == typeof(string))
                    {
                        o.MapFrom(s => "-");
                    }
                });

            CreateMap<CreateLeadHomeRequest, LeadHome>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(x => x.FullName.ToUpper()));
            CreateMap<UpdateLeadHomeRequest, LeadHome>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(x => x.FullName.ToUpper()));
            CreateMap<LeadHomeAddressDto, LeadHomeAddress>();

            CreateMap<CreateLeadOkVayRequest, LeadOkVay>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(x => x.FullName.ToUpper()));
            CreateMap<UpdateLeadOkVayRequest, LeadOkVay>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(x => x.FullName.ToUpper()));
            CreateMap<LeadOkVayAddressDto, LeadOkVayAddress>();

            CreateMap<CreateLeadVbiRequest, LeadVbi>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(x => x.FullName.ToUpper()));
            CreateMap<UpdateLeadVbiRequest, LeadVbi>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(x => x.FullName.ToUpper()));
            CreateMap<LeadVbiAddressDto, LeadVbiAddress>();
            CreateMap<CreateHyperLeadDto, HyperLead>()
                .ForMember(dest => dest.ConversionId, src => src.MapFrom(x => x.conversion_id))
                .ForMember(dest => dest.ClickId, src => src.MapFrom(x => x.click_id))
                .ForMember(dest => dest.ClickTime, src => src.MapFrom(x => x.click_time))
                .ForMember(dest => dest.ConversionSaleAmount, src => src.MapFrom(x => x.conversion_sale_amount))
                .ForMember(dest => dest.ConversionStatusCode, src => src.MapFrom(x => x.conversion_status_code))
                .ForMember(dest => dest.ConversionStatus, src => src.MapFrom(x => x.conversion_status))
                .ForMember(dest => dest.ConversionPublisherPayout, src => src.MapFrom(x => x.conversion_publisher_payout))
                .ForMember(dest => dest.ConversionTime, src => src.MapFrom(x => x.conversion_time))
                .ForMember(dest => dest.ConversionModifiedTime, src => src.MapFrom(x => x.conversion_modified_time))
                .ForMember(dest => dest.ProductUrl, src => src.MapFrom(x => x.product_url))
                .ForMember(dest => dest.ProductName, src => src.MapFrom(x => x.product_name))
                .ForMember(dest => dest.ProductSku, src => src.MapFrom(x => x.product_sku))
                .ForMember(dest => dest.ProductCategory, src => src.MapFrom(x => x.product_category))
                .ForMember(dest => dest.ProductCategoryId, src => src.MapFrom(x => x.product_category_id))
                .ForMember(dest => dest.OfferId, src => src.MapFrom(x => x.offer_id))
                .ForMember(dest => dest.TransactionId, src => src.MapFrom(x => x.transaction_id))
                .ForMember(dest => dest.StatusMessage, src => src.MapFrom(x => x.status_message))
                .ForMember(dest => dest.AffSub1, src => src.MapFrom(x => x.aff_sub1))
                .ForMember(dest => dest.AffSub2, src => src.MapFrom(x => x.aff_sub2))
                .ForMember(dest => dest.AffSub3, src => src.MapFrom(x => x.aff_sub3))
                .ForMember(dest => dest.AffSub4, src => src.MapFrom(x => x.aff_sub4));

            CreateMap<HyperLead, HyperLead>()
                .ForMember(dest => dest.Id, src => src.Ignore())
                .ForMember(dest => dest.CreatedDate, src => src.Ignore())
                .ForMember(dest => dest.SaleInfomation, src => src.Ignore())
                .ForMember(dest => dest.TeamLeadInfo, src => src.Ignore())
                .ForMember(dest => dest.AsmInfo, src => src.Ignore())
                .ForMember(dest => dest.PosInfo, src => src.Ignore())
                .ForMember(dest => dest.SaleChanelInfo, src => src.Ignore())
                .ForMember(dest => dest.Creator, src => src.Ignore());
        }
    }
}
