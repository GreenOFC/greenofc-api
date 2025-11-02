using _24hplusdotnetcore.Models;
using FluentValidation;
using System.Linq;

namespace _24hplusdotnetcore.Validators
{
    public class LeadEcCustomerValidator: AbstractValidator<Customer>
    {
        public LeadEcCustomerValidator()
        {
            RuleFor(x => x.Personal).NotNull().WithMessage("Thông tin cá nhân").SetValidator(new LeadEcPersionalValidator());
            RuleFor(x => x.ResidentAddress).NotNull().WithMessage("Địa chỉ thường trú").SetValidator(new LeadEcAddressValidator());
            RuleFor(x => x.TemporaryAddress).NotNull().WithMessage("Địa chỉ tạm trú").SetValidator(new LeadEcAddressValidator());
            RuleFor(x => x.Loan).NotNull().WithMessage("Thông tin sản phẩm vay").SetValidator(new LeadEcLoanValidator());
            RuleFor(x => x.Working).NotNull().WithMessage("Thông tin công việc").SetValidator(new LeadEcWorkingValidator());
            RuleFor(x => x.DisbursementInformation).NotNull().WithMessage("Thông tin giải ngân").SetValidator(new LeadEcDisbursementInformationValidator());
            RuleFor(x => x.Referees).Must(x => x != null && x.Count() > 1).WithMessage("Thông tin tham chiếu");
            RuleForEach(model => model.Referees).SetValidator(new LeadEcRefereeValidator());
        }
    }

    public class LeadEcPersionalValidator : AbstractValidator<Personal>
    {
        public LeadEcPersionalValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Họ và tên")
                .NotEmpty().WithMessage("Họ và tên");
            RuleFor(x => x.IdCard)
                .NotNull().WithMessage("Số CMND/CCCD")
                .NotEmpty().WithMessage("Số CMND/CCCD");
            RuleFor(x => x.IdCardProvinceId)
                .NotNull().WithMessage("Nơi cấp CMND/CCCD")
                .NotEmpty().WithMessage("Nơi cấp CMND/CCCD");
            RuleFor(x => x.IdCardDate)
                .NotNull().WithMessage("Ngày cấp CMND/CCCD")
                .NotEmpty().WithMessage("Ngày cấp CMND/CCCD");
            RuleFor(x => x.Gender)
                .NotNull().WithMessage("Giới tính")
                .NotEmpty().WithMessage("Giới tính");
            RuleFor(x => x.DateOfBirth)
                .NotNull().WithMessage("Ngày sinh")
                .NotEmpty().WithMessage("Ngày sinh");
            RuleFor(x => x.Phone)
                .NotNull().WithMessage("SĐT")
                .NotEmpty().WithMessage("SĐT");
        }
    }

    public class LeadEcAddressValidator : AbstractValidator<Address>
    {
        public LeadEcAddressValidator()
        {
            RuleFor(x => x.ProvinceId).NotNull().NotEmpty().WithMessage("Tỉnh/Thành phố");
            RuleFor(x => x.DistrictId).NotNull().NotEmpty().WithMessage("Quận/huyện");
            RuleFor(x => x.WardId).NotNull().NotEmpty().WithMessage("Phường/xã");
            RuleFor(x => x.Street).NotNull().NotEmpty().WithMessage("Tên đường, số nhà");
        }
    }

    public class LeadEcLoanValidator : AbstractValidator<Loan>
    {
        public LeadEcLoanValidator()
        {
            RuleFor(x => x.ProductId)
                .NotNull().WithMessage("Sản phẩm")
                .NotEmpty().WithMessage("Sản phẩm");
            RuleFor(x => x.Amount)
                .NotNull().WithMessage("Số tiền đề nghị vay")
                .NotEmpty().WithMessage("Số tiền đề nghị vay");
            RuleFor(x => x.TermId)
                .NotNull().WithMessage("Kỳ hạn vay")
                .NotEmpty().WithMessage("Kỳ hạn vay");
            RuleFor(x => x.Purpose)
                .NotNull().WithMessage("Mục đích vay")
                .NotEmpty().WithMessage("Mục đích vay");
        }
    }

    public class LeadEcWorkingValidator : AbstractValidator<Working>
    {
        public LeadEcWorkingValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotNull().WithMessage("Tên công ty")
                .NotEmpty().WithMessage("Tên công ty");
            RuleFor(x => x.EmploymentStatusId)
                .NotNull().WithMessage("Hình thức làm việc")
                .NotEmpty().WithMessage("Hình thức làm việc");
            RuleFor(x => x.Income)
                .NotNull().WithMessage("Thu nhập chính")
                .NotEmpty().WithMessage("Thu nhập chính");
            RuleFor(x => x.MonthlyExpenese)
                .NotNull().WithMessage("Tổng chi tiêu hàng tháng của bạn")
                .NotEmpty().WithMessage("Tổng chi tiêu hàng tháng của bạn");
            RuleFor(x => x.CompanyAddress).NotNull().WithMessage("Thông tin công việc").SetValidator(new LeadEcAddressValidator());
        }
    }

    public class LeadEcDisbursementInformationValidator: AbstractValidator<DisbursementInformation>
    {
        public LeadEcDisbursementInformationValidator()
        {
            RuleFor(x => x.DisbursementMethodId)
                .NotNull().WithMessage("Phương thức giải ngân")
                .NotEmpty().WithMessage("Phương thức giải ngân");
        }
    }

    public class LeadEcRefereeValidator : AbstractValidator<Referee>
    {
        public LeadEcRefereeValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Tên người tham chiếu")
                .NotEmpty().WithMessage("Tên người tham chiếu");
            RuleFor(x => x.Phone)
                .NotNull().NotEmpty().WithMessage("SĐT người tham chiếu")
                .NotEmpty().WithMessage("SĐT người tham chiếu");
            RuleFor(x => x.RelationshipId)
                .NotNull().WithMessage("Mối quan hệ người tham chiếu")
                .NotEmpty().WithMessage("Mối quan hệ người tham chiếu");
        }
    }

}
