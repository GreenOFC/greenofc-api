using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    [BsonIgnoreExtraElements]
    public class GetDetailLeadPtfResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Status { get; set; }
        public string ContractCode { get; set; }
        public string ProductLine { get; set; }
        public string CustomerType { get; set; }
        public string CustomerTypeId { get; set; }
        public LeadPtfPersonalDto Personal { get; set; }
        public LeadPtfWorkingDto Working { get; set; }
        public string FamilyBookNo { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
        public LeadPtfAddressDto TemporaryAddress { get; set; }
        public LeadPtfShortAddressDto ResidentAddress { get; set; }
        public IEnumerable<LeadPtfRefereeDto> Referees { get; set; }
        public LeadPtfLoanDto Loan { get; set; }
        public LeadPtfDisbursementInformationDto DisbursementInformation { get; set; }
        public IEnumerable<LeadPtfGroupDocumentDto> Documents { get; set; }
        public IEnumerable<LeadPtfGroupDocumentDto> ReturnDocuments { get; set; }
        public LeadPtfResultDto Result { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }
        public SaleDto SaleInfo { get; set; }
        public PosInfoDto PosInfo { get; set; }

        public TeamLeadDto TeamLeadInfo { get; set; }

        public SaleChanelDto SaleChanelInfo { get; set; }
    }
}
