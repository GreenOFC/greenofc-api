using _24hplusdotnetcore.Models;
using FluentValidation;
using System.Linq;

namespace _24hplusdotnetcore.Validators
{
    public class LeadPtfValidation : AbstractValidator<Customer>
    {
        public LeadPtfValidation()
        {
            RuleFor(x => x.Personal).NotNull().WithMessage("Thông tin khách hàng").SetValidator(new LeadPtfPersionalValidator());
            RuleFor(x => x.Working).NotNull().WithMessage("Thông tin công việc").SetValidator(new LeadPtfWorkingValidator());
            RuleFor(x => x.TemporaryAddress).NotNull().WithMessage("Địa chỉ hiện tại").SetValidator(new CimbAddressValidator());
            RuleFor(x => x.ResidentAddress).NotNull().WithMessage("Địa chỉ hiện tại").SetValidator(new CimbAddressValidator());
            RuleFor(x => x.FamilyBookNo).NotEmpty().WithMessage("Sổ hộ khẩu");
            RuleFor(x => x.Referees).Must(x => x != null && x.Count() > 1).WithMessage("Thông tin tham chiếu");
            RuleForEach(model => model.Referees).SetValidator(new LeadPtfRefereeValidator());
            RuleFor(x => x.Loan).NotNull().WithMessage("Thông tin khoản vay").SetValidator(new LeadPtfLoanValidator());
            RuleFor(x => x.DisbursementInformation).NotNull().WithMessage("Thông tin tài khoản giải ngân").SetValidator(new LeadPtfDisbursementInformationValidator());
        }
    }

    public class LeadPtfPersionalValidator : AbstractValidator<Personal>
    {
        public LeadPtfPersionalValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100).WithMessage("Họ và tên");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Ngày sinh");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Giới tính");
            RuleFor(x => x.IdCard).NotEmpty().MaximumLength(20).WithMessage("Số CMND/CCCD");
            RuleFor(x => x.IdCardDate).NotEmpty().WithMessage("Ngày cấp CMND/CCCD");
            RuleFor(x => x.IdCardProvinceId).NotEmpty().WithMessage("Nơi cấp CMND/CCCD");
            RuleFor(x => x.OldIdCard).MaximumLength(20).WithMessage("Số CMND/CCCD cũ");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("SĐT");
            RuleFor(x => x.MaritalStatusId).NotEmpty().WithMessage("Tình trạng hôn nhân");
            RuleFor(x => x.NoOfDependent).NotEmpty().WithMessage("Số người phụ thuộc");
            // RuleFor(x => x.DependentTypes).NotEmpty().WithMessage("Người phụ thuộc");
            RuleFor(x => x.EducationLevelId).NotEmpty().WithMessage("Học vấn");
        }
    }

    public class LeadPtfWorkingValidator : AbstractValidator<Working>
    {
        public LeadPtfWorkingValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().WithMessage("Tên công ty");
            RuleFor(x => x.PositionId).NotEmpty().WithMessage("Vị trí làm việc");
            RuleFor(x => x.CompanyAddress).NotEmpty().WithMessage("Địa chỉ công ty").SetValidator(new LeadPtfAddressValidator()); ;
            RuleFor(x => x.JobId).NotEmpty().WithMessage("Nghề nghiệp");
            RuleFor(x => x.CompanyPhone).NotEmpty().WithMessage("Điện thoại bàn");
            RuleFor(x => x.DateStartWork).NotEmpty().WithMessage("Ngày bắt đầu làm việc/đi học lại công ty/trường học gần nhất");
            RuleFor(x => x.Income).NotNull().NotEmpty().WithMessage("Mức thu nhập VNĐ/tháng");
        }
    }

    public class LeadPtfAddressValidator : AbstractValidator<Address>
    {
        public LeadPtfAddressValidator()
        {
            RuleFor(x => x.ProvinceId).NotEmpty().WithMessage("Tỉnh/Thành phố");
            RuleFor(x => x.DistrictId).NotEmpty().WithMessage("Quận/huyện");
            RuleFor(x => x.WardId).NotEmpty().WithMessage("Phường/xã");
            RuleFor(x => x.Street).NotEmpty().WithMessage("Tên đường, số nhà");
        }
    }

    public class LeadPtfRefereeValidator : AbstractValidator<Referee>
    {
        public LeadPtfRefereeValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Tên người tham chiếu");
            RuleFor(x => x.Phone).NotNull().NotEmpty().WithMessage("SĐT người tham chiếu");
            RuleFor(x => x.RelationshipId).NotNull().NotEmpty().WithMessage("Mối quan hệ người tham chiếu");
        }
    }

    public class LeadPtfLoanValidator : AbstractValidator<Loan>
    {
        public LeadPtfLoanValidator()
        {
            // RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Loại sản phẩm");
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Tên sản phẩm");
            RuleFor(x => x.Amount).NotEmpty().WithMessage("Số tiền/Hạn mức đề nghị vay");
            RuleFor(x => x.PurposeId).NotEmpty().WithMessage("Mục đích vay");
            RuleFor(x => x.TermId).NotEmpty().WithMessage("Kỳ hạn vay");
            RuleFor(x => x.TotalPaymentsToCreditInstitution).NotNull().WithMessage("Thanh toán cho khoản vay khác");
            RuleFor(x => x.NumberOfCreditInstitutionsInDebt).NotNull().WithMessage("Số lượng Tổ chức tín dụng (TCTD) đang quan hệ tín dụng");
            RuleFor(x => x.TimeToBeAbleToAnswerThePhone).NotEmpty().WithMessage("Thời gian có thể nghe máy");
        }
    }

    public class LeadPtfDisbursementInformationValidator : AbstractValidator<DisbursementInformation>
    {
        public LeadPtfDisbursementInformationValidator()
        {
            RuleFor(x => x.DisbursementMethodId).NotEmpty().WithMessage("Phương thức giải ngân");
        }
    }
}
