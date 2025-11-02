using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.MAFC;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using AutoMapper;
using System.Linq;

namespace _24hplusdotnetcore.Mappings
{
    public class MAFCMappingProfile : Profile
    {
        public MAFCMappingProfile()
        {
            #region Customer

            CreateMap<Customer, MAFCInputQDERestRequest>()
                .ForMember(dest => dest.In_schemeid, src => src.MapFrom(x => x.Loan.ProductId))
                .ForMember(dest => dest.In_totalloanamountreq, src => src.MapFrom(x => x.Loan.Amount.Replace(",", string.Empty)))
                .ForMember(dest => dest.In_tenure, src => src.MapFrom(x => x.Loan.Term))
                .ForMember(dest => dest.In_salesofficer, src => src.MapFrom(x => x.SaleInfo.MAFCId))
                .ForMember(dest => dest.In_loanpurpose, src => src.MapFrom(x => x.Loan.GetMAFCPurpose()))
                .ForMember(dest => dest.In_laa_app_ins_applicable, src => src.MapFrom(x => x.Loan.BuyInsurance == true ? "Y" : "N"))
                .ForMember(dest => dest.In_priority_c, src => src.MapFrom(x => x.Working.GetMAFCPriority()))
                .ForMember(dest => dest.In_title, src => src.MapFrom(x => x.Personal.Title))
                .ForMember(dest => dest.In_fname, src => src.MapFrom(x => x.Personal.GetFamilyName()))
                .ForMember(dest => dest.In_mname, src => src.MapFrom(x => x.Personal.GetMiddleName()))
                .ForMember(dest => dest.In_lname, src => src.MapFrom(x => x.Personal.GetName()))
                .ForMember(dest => dest.In_gender, src => src.MapFrom(x => x.Personal.Gender == "Nam" ? "M" : "F"))
                .ForMember(dest => dest.In_nationalid, src => src.MapFrom(x => x.Personal.IdCard))
                .ForMember(dest => dest.In_dob, src => src.MapFrom(x => x.Personal.DateOfBirth))
                .ForMember(dest => dest.In_constid, src => src.MapFrom(x => x.Working.GetMAFCConsti()))
                .ForMember(dest => dest.In_tax_code, src => src.MapFrom(x => x.Working.CompanyTaxCode + (!string.IsNullOrEmpty(x.Working.CompanyTaxSubCode) ? "-" + x.Working.CompanyTaxSubCode : "")))
                .ForMember(dest => dest.In_presentjobyear, src => src.MapFrom(x => x.Working.CompanyAddress.DurationYear))
                .ForMember(dest => dest.In_presentjobmth, src => src.MapFrom(x => x.Working.CompanyAddress.DurationMonth))
                .ForMember(dest => dest.In_addresstype, src => src.MapFrom(x => x.Working.CompanyAddress.Type))
                .ForMember(dest => dest.In_addressline, src => src.MapFrom(x => x.Working.CompanyAddress.Street.ToUpper()))
                .ForMember(dest => dest.In_city, src => src.MapFrom(x => x.Working.CompanyAddress.ProvinceId))
                .ForMember(dest => dest.In_district, src => src.MapFrom(x => x.Working.CompanyAddress.DistrictId))
                .ForMember(dest => dest.In_ward, src => src.MapFrom(x => x.Working.CompanyAddress.WardId))
                .ForMember(dest => dest.In_phone, src => src.MapFrom(x => x.Working.CompanyAddress.FixedPhone))
                .ForMember(dest => dest.In_others, src => src.MapFrom(x => x.Working.CompanyName.ToUpper()))
                .ForMember(dest => dest.In_position, src => src.MapFrom(x => x.Working.Position.ToUpper()))
                .ForMember(dest => dest.Reference, src => src.MapFrom(x => x.Referees))
                .ForMember(dest => dest.In_amount, src => src.MapFrom(x => x.Working.Income.Replace(",", string.Empty)))
                .ForMember(dest => dest.In_accountbank, src => src.MapFrom(x => x.Working.GetMAFCIncomeMethod()))
                ;

            CreateMap<Address, MAFCInputQDEAddressDto>()
                .ForMember(dest => dest.In_addresstype, src => src.MapFrom(x => x.Type))
                .ForMember(dest => dest.In_propertystatus, src => src.MapFrom(x => x.GetMAFCPropertyStatus()))
                .ForMember(dest => dest.In_address1stline, src => src.MapFrom(x => x.Street.ToUpper()))
                .ForMember(dest => dest.In_city, src => src.MapFrom(x => x.ProvinceId))
                .ForMember(dest => dest.In_district, src => src.MapFrom(x => x.DistrictId))
                .ForMember(dest => dest.In_ward, src => src.MapFrom(x => x.WardId))
                .ForMember(dest => dest.In_roomno, src => src.MapFrom(x => string.Empty + x.RoomNo.ToUpper()))
                .ForMember(dest => dest.In_landlord, src => src.MapFrom(x => (string.Empty + x.LandLordName).ToUpper()))
                .ForMember(dest => dest.In_stayduratcuradd_y, src => src.MapFrom(x => x.DurationYear))
                .ForMember(dest => dest.In_stayduratcuradd_m, src => src.MapFrom(x => x.DurationMonth))
                .ForMember(dest => dest.In_mailingaddress, src => src.MapFrom(x => x.MailAddress))
                .ForMember(dest => dest.In_mobile, src => src.MapFrom(x => "")) // @todo
                .ForMember(dest => dest.In_landmark, src => src.MapFrom(x => x.LandMark.ToUpper()))
                ;

            CreateMap<Referee, MAFCInputQDEReferenceDto>()
                .ForMember(dest => dest.In_title, src => src.MapFrom(x => x.Title))
                .ForMember(dest => dest.In_refereename, src => src.MapFrom(x => x.Name.ToUpper()))
                .ForMember(dest => dest.In_phone_1, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.In_refereerelation, src => src.MapFrom(x => x.RelationshipId))
                ;

            CreateMap<Customer, MAFCInputDDERestRequest>()
                .ForMember(dest => dest.In_appid, src => src.MapFrom(x => x.MAFCId))
                .ForMember(dest => dest.In_maritalstatus, src => src.MapFrom(x => x.Personal.GetMAFCMaritalStatus()))

                .ForMember(dest => dest.In_eduqualify, src => src.MapFrom(x => x.Personal.GetMAFCEducation()))
                .ForMember(dest => dest.In_noofdependentin, src => src.MapFrom(x => x.Personal.NoOfDependent))
                .ForMember(dest => dest.In_paymentchannel, src => src.MapFrom(x => MAFCDataEntry.ATProduct.Where(n => n == x.Loan.ProductId).Any() ? "T" : "C"))
                .ForMember(dest => dest.In_nationalidissuedate, src => src.MapFrom(x => x.Personal.IdCardDate))
                .ForMember(dest => dest.In_familybooknumber, src => src.MapFrom(x => x.FamilyBookNo.ToUpper() ?? ""))
                .ForMember(dest => dest.In_idissuer, src => src.MapFrom(x => x.Personal.IdCardProvince.ToUpper()))
                .ForMember(dest => dest.In_spousename, src => src.MapFrom(x => x.Spouse.Name.ToUpper()))
                .ForMember(dest => dest.In_spouse_id_c, src => src.MapFrom(x => x.Spouse.IdCard))
                .ForMember(dest => dest.In_bankname, src => src.MapFrom(x => x.BankInfo.Id))
                .ForMember(dest => dest.In_bankbranch, src => src.MapFrom(x => x.BankInfo.Branch.ToUpper()))
                .ForMember(dest => dest.In_accno, src => src.MapFrom(x => x.BankInfo.AccountNo))
                .ForMember(dest => dest.In_dueday, src => src.MapFrom(x => !string.IsNullOrEmpty(x.Loan.PaymentDate) ? x.Loan.PaymentDate : x.Working.DueDay))
                .ForMember(dest => dest.In_notedetails, src => src.MapFrom(x => x.Personal.OldIdCard))
                ;

            CreateMap<Customer, MAFCUpdateInfoModel>()
                .ForMember(dest => dest.CustomerId, src => src.MapFrom(x => x.Id))

                .ForMember(dest => dest.In_schemeid, src => src.MapFrom(x => x.Loan.ProductId))
                .ForMember(dest => dest.In_totalloanamountreq, src => src.MapFrom(x => x.Loan.Amount.Replace(",", string.Empty)))
                .ForMember(dest => dest.In_tenure, src => src.MapFrom(x => x.Loan.Term))
                .ForMember(dest => dest.In_laa_app_ins_applicable, src => src.MapFrom(x => x.Loan.BuyInsurance == true ? "Y" : "N"))

                .ForMember(dest => dest.In_loanpurpose, src => src.MapFrom(x => x.Loan.GetMAFCPurpose()))
                .ForMember(dest => dest.In_priority_c, src => src.MapFrom(x => x.Working.GetMAFCPriority()))
                .ForMember(dest => dest.In_title, src => src.MapFrom(x => x.Personal.Title))
                .ForMember(dest => dest.In_fname, src => src.MapFrom(x => x.Personal.GetFamilyName()))
                .ForMember(dest => dest.In_mname, src => src.MapFrom(x => x.Personal.GetMiddleName()))
                .ForMember(dest => dest.In_lname, src => src.MapFrom(x => x.Personal.GetName()))
                .ForMember(dest => dest.In_gender, src => src.MapFrom(x => x.Personal.Gender == "Nam" ? "M" : "F"))
                .ForMember(dest => dest.In_nationalid, src => src.MapFrom(x => x.Personal.IdCard))
                .ForMember(dest => dest.In_dob, src => src.MapFrom(x => x.Personal.DateOfBirth))
                .ForMember(dest => dest.In_tax_code, src => src.MapFrom(x => x.Working.CompanyTaxCode + (!string.IsNullOrEmpty(x.Working.CompanyTaxSubCode) ? "-" + x.Working.CompanyTaxSubCode : "")))
                .ForMember(dest => dest.In_presentjobyear, src => src.MapFrom(x => x.Working.CompanyAddress.DurationYear))
                .ForMember(dest => dest.In_presentjobmth, src => src.MapFrom(x => x.Working.CompanyAddress.DurationMonth))

                .ForMember(dest => dest.In_others, src => src.MapFrom(x => x.Working.CompanyName.ToUpper()))
                .ForMember(dest => dest.In_position, src => src.MapFrom(x => x.Working.Position.ToUpper()))
                .ForMember(dest => dest.In_amount, src => src.MapFrom(x => x.Working.Income.Replace(",", string.Empty)))
                .ForMember(dest => dest.In_accountbank, src => src.MapFrom(x => x.Working.GetMAFCIncomeMethod() ?? ""))
                .ForMember(dest => dest.In_maritalstatus, src => src.MapFrom(x => x.Personal.GetMAFCMaritalStatus() ?? ""))
                .ForMember(dest => dest.In_eduqualify, src => src.MapFrom(x => x.Personal.GetMAFCEducation()))
                .ForMember(dest => dest.In_noofdependentin, src => src.MapFrom(x => x.Personal.NoOfDependent))
                .ForMember(dest => dest.In_paymentchannel, src => src.MapFrom(x => MAFCDataEntry.ATProduct.Where(n => n == x.Loan.ProductId).Any() ? "T" : "C"))
                .ForMember(dest => dest.In_nationalidissuedate, src => src.MapFrom(x => x.Personal.IdCardDate))
                .ForMember(dest => dest.In_familybooknumber, src => src.MapFrom(x => x.FamilyBookNo.ToUpper() ?? ""))
                .ForMember(dest => dest.In_idissuer, src => src.MapFrom(x => x.Personal.IdCardProvince.ToUpper()))
                .ForMember(dest => dest.In_spousename, src => src.MapFrom(x => x.Spouse.Name.ToUpper()))
                .ForMember(dest => dest.In_spouse_id_c, src => src.MapFrom(x => x.Spouse.IdCard))
                .ForMember(dest => dest.In_bankname, src => src.MapFrom(x => x.BankInfo.Id))
                .ForMember(dest => dest.In_bankbranch, src => src.MapFrom(x => x.BankInfo.Branch.ToUpper()))
                .ForMember(dest => dest.In_accno, src => src.MapFrom(x => x.BankInfo.AccountNo))
                .ForMember(dest => dest.Reference, src => src.MapFrom(x => x.Referees))
                ;


            CreateMap<Address, MAFCUpdateAddressInfoModel>()
                .ForMember(dest => dest.In_addresstype, src => src.MapFrom(x => x.Type))
                .ForMember(dest => dest.In_propertystatus, src => src.MapFrom(x => x.GetMAFCPropertyStatus()))
                .ForMember(dest => dest.In_address1stline, src => src.MapFrom(x => x.Street.ToUpper()))
                .ForMember(dest => dest.In_city, src => src.MapFrom(x => x.ProvinceId))
                .ForMember(dest => dest.In_district, src => src.MapFrom(x => x.DistrictId))
                .ForMember(dest => dest.In_ward, src => src.MapFrom(x => x.WardId))
                .ForMember(dest => dest.In_roomno, src => src.MapFrom(x => string.Empty + x.RoomNo.ToUpper()))
                .ForMember(dest => dest.In_mobile, src => src.MapFrom(x => ""))
                ;

            CreateMap<Referee, MAFCUpdateReferenceInfoModel>()
                .ForMember(dest => dest.In_title, src => src.MapFrom(x => x.Title))
                .ForMember(dest => dest.In_refereename, src => src.MapFrom(x => x.Name.ToUpper()))
                .ForMember(dest => dest.In_phone_1, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.In_refereerelation, src => src.MapFrom(x => x.RelationshipId))
                ;

            #endregion

            #region MAFC

            CreateMap<MAFCUpdateInfoModel, MAFCInputUpdateRestRequest>()
                .ForMember(dest => dest.Reference, src => src.MapFrom(x => x.Reference))
                .ForMember(dest => dest.Address, src => src.MapFrom(x => x.Address))
                ;

            CreateMap<MAFCInputQDEReferenceDto, MAFCUpdateReferenceInfoModel>().ReverseMap();
            CreateMap<MAFCInputQDEAddressDto, MAFCUpdateAddressInfoModel>().ReverseMap();

            #endregion

            CreateMap<MafcPersonalDto, Personal>().ReverseMap();
            CreateMap<MafcAddressDto, Address>().ReverseMap();
            CreateMap<MafcLoanDto, Loan>().ReverseMap();
            CreateMap<MafcWorkingDto, Working>().ReverseMap();
            CreateMap<MafcRefereeDto, Referee>().ReverseMap();
            CreateMap<MafcBankInfoDto, BankInfo>().ReverseMap();
            CreateMap<MafcOtherInfoDto, OtherInfo>().ReverseMap();
            CreateMap<MafcResultDto, Result>().ReverseMap();
            CreateMap<MafcGroupDocumentDto, GroupDocument>().ReverseMap();
            CreateMap<MafcDocumentUploadDto, DocumentUpload>().ReverseMap();
            CreateMap<MafcUploadedMediaDto, UploadedMedia>().ReverseMap();
            CreateMap<Sale, LeadMafcSaleDto>();
            CreateMap<OldCustomer, Customer>().ReverseMap();
            CreateMap<OldCustomer, MafcOldCustomerDto>().ReverseMap();

            CreateMap<CreateMafcStep1Request, Customer>()
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenA))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<UpdateMafcStep1Request, Customer>();
            CreateMap<UpdateMafcStep2Request, Customer>();
            CreateMap<UpdateMafcStep3Request, Customer>();
            CreateMap<UpdateMafcStep4Request, Customer>();
            CreateMap<UpdateMafcStep5Request, Customer>();
            CreateMap<UpdateMafcStep6Request, Customer>()
                .ForMember(dest => dest.Status, opt => opt.Ignore());
            CreateMap<UpdateMafcRecordFileRequest, Customer>();

            CreateMap<CreateMafcStep1WebRequest, Customer>()
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenA))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<UpdateMafcStep1WebRequest, Customer>();
            CreateMap<UpdateMafcStep3WebRequest, Customer>();

            CreateMap<Customer, GetMafcDetailResponse>()
                .ForMember(dest => dest.Working, opt => opt.MapFrom(src => src.Working ?? new Working()));

            CreateMap<Customer, GetOldMafcResponse>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Personal.Name));

        }
    }
}
