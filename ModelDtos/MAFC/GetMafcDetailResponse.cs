using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    public class GetMafcDetailResponse
    {
        public string Id { get; set; }
        public int MAFCId { get; set; }
        public string Status { get; set; }
        public string ContractCode { get; set; }
        public string CustomerType { get; set; }
        public string CustomerTypeId { get; set; }
        public MafcOldCustomerDto OldCustomer { get; set; }
        public MafcPersonalDto Personal { get; set; }
        public MafcRefereeDto Spouse { get; set; }
        public IEnumerable<MafcRefereeDto> Referees { get; set; }
        public string ProductLine { get; set; }
        public string CaseNote { get; set; }
        public MafcAddressDto ResidentAddress { get; set; }
        public MafcAddressDto TemporaryAddress { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
        public string FamilyBookNo { get; set; }
        public MafcLoanDto Loan { get; set; }
        public MafcWorkingDto Working { get; set; }
        public MafcBankInfoDto BankInfo { get; set; }
        public MafcOtherInfoDto OtherInfo { get; set; }
        public MafcResultDto Result { get; set; }
        public string UserName { get; set; }
        public LeadMafcSaleDto SaleInfo { get; set; }
        public IEnumerable<MafcGroupDocumentDto> Documents { get; set; }
        public IEnumerable<MafcGroupDocumentDto> ReturnDocuments { get; set; }
        public MafcUploadedMediaDto RecordFile { get; set; }
        public string Creator { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class MafcOldCustomerDto
    {
        public string Id { get; set; }
        public int MAFCId { get; set; }
        public string Status { get; set; }
        public string ContractCode { get; set; }
    }
}
