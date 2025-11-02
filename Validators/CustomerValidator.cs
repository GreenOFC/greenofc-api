using _24hplusdotnetcore.Models;
using FluentValidation;
using System.Linq;

namespace _24hplusdotnetcore.Validators
{
    public class CustomerValidator
    {
    }

    public class CimbCustomerValidator : AbstractValidator<Customer>
    {
        public CimbCustomerValidator()
        {
            RuleFor(x => x.Personal).NotNull().WithMessage("Thông tin khách hàng").SetValidator(new CimbPersionalValidator());
            RuleFor(x => x.Working).NotNull().WithMessage("Thông tin công việc").SetValidator(new CimbWorkingValidator());
            RuleFor(x => x.Referees).Must(x => x != null && x.Count() > 1).WithMessage("Thông tin tham chiếu");
            RuleForEach(model => model.Referees).SetValidator(new CimbRefereeValidator());
            RuleFor(x => x.ResidentAddress).NotNull().WithMessage("Địa chỉ hiện tại").SetValidator(new CimbAddressValidator());
            RuleFor(x => x.Loan).NotNull().WithMessage("Thông tin khoản vay").SetValidator(new CimbLoanValidator());
            RuleFor(x => x.Documents).Must(x => x != null && x.Count() > 2).WithMessage("Hình ảnh khách hàng");
            RuleForEach(x => x.Documents).SetValidator(new CimbGroupDocumentValidator());
        }
    }

    public class CimbPersionalValidator: AbstractValidator<Personal>
    {
        public CimbPersionalValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Họ và tên");
            RuleFor(x => x.IdCard).NotEmpty().WithMessage("Số CMND/CCCD");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Ngày sinh");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Giới tính");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("SĐT");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email");
            RuleFor(x => x.MaritalStatusId).NotEmpty().WithMessage("Tình trạng hôn nhân");
            RuleFor(x => x.EducationLevelId).NotEmpty().WithMessage("Học vấn");
            RuleFor(x => x.IsEmailVerified).Must(x => x).WithMessage("Xác nhận email");
            RuleFor(x => x.IsPhoneVerified).Must(x => x).WithMessage("Xác nhận SĐT");
        }
    }

    public class CimbWorkingValidator: AbstractValidator<Working>
    {
        public CimbWorkingValidator()
        {
            RuleFor(x => x.CompanyName).NotNull().NotEmpty().WithMessage("Tên công ty");
            RuleFor(x => x.EmploymentStatusId).NotNull().NotEmpty().WithMessage("Hình thức làm việc");
            RuleFor(x => x.Income).NotNull().NotEmpty().WithMessage("Mức thu nhập VNĐ/tháng");
        }
    }

    public class CimbAddressValidator: AbstractValidator<Address>
    {
        public CimbAddressValidator()
        {
            RuleFor(x => x.ProvinceId).NotNull().NotEmpty().WithMessage("Tỉnh/Thành phố");
            RuleFor(x => x.DistrictId).NotNull().NotEmpty().WithMessage("Quận/huyện");
            RuleFor(x => x.WardId).NotNull().NotEmpty().WithMessage("Phường/xã");
            RuleFor(x => x.Street).NotNull().NotEmpty().WithMessage("Tên đường, số nhà");
        }
    }

    public class CimbLoanValidator: AbstractValidator<Loan>
    {
        public CimbLoanValidator()
        {
            RuleFor(x => x.Amount).NotNull().NotEmpty().WithMessage("Số tiền đề nghị vay");
            RuleFor(x => x.TermId).NotNull().NotEmpty().WithMessage("Kỳ hạn vay");
            RuleFor(x => x.PurposeId).NotNull().NotEmpty().WithMessage("Mục đích vay");
        }
    }

    public class CimbRefereeValidator : AbstractValidator<Referee>
    {
        public CimbRefereeValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Tên người tham chiếu");
            RuleFor(x => x.Phone).NotNull().NotEmpty().WithMessage("SĐT người tham chiếu");
            RuleFor(x => x.RelationshipId).NotNull().NotEmpty().WithMessage("Mối quan hệ người tham chiếu");
        }
    }

    public class CimbGroupDocumentValidator: AbstractValidator<GroupDocument>
    {
        public CimbGroupDocumentValidator()
        {
            RuleFor(x => x.Documents).Must(x => x != null && x.Count() > 0).WithMessage(x => x.GroupName);
            RuleForEach(x => x.Documents).SetValidator(new CimbDocumentUploadValidator());
        }
    }

    public class CimbDocumentUploadValidator : AbstractValidator<DocumentUpload>
    {
        public CimbDocumentUploadValidator()
        {
            RuleFor(x => x.UploadedMedias).Must(x => x != null && x.Count() > 0).WithMessage(x => x.DocumentName);
            RuleForEach(x => x.UploadedMedias).SetValidator(x => new CimbUploadedMediaValidator(x.DocumentName));
        }
    }

    public class CimbUploadedMediaValidator: AbstractValidator<UploadedMedia>
    {
        public CimbUploadedMediaValidator(string documentName)
        {
            RuleFor(x => x.Uri).NotNull().NotEmpty().WithMessage(documentName);
        }
    }
}
