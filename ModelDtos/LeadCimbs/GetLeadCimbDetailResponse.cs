using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class GetLeadCimbDetailResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CimbCode { get; set; }
        public string Status { get; set; }
        public string ContractCode { get; set; }
        public bool IsCheckOnboardCimb { get; set; }
        public CimbPersonalResponseDto Personal { get; set; }
        public CimbWorkingDto Working { get; set; }
        public IEnumerable<CimbReferenceDto> Referees { get; set; }
        public CimbAddressDto ResidentAddress { get; set; }
        public CimbLoanDto Loan { get; set; }
        public IEnumerable<CimbGroupDocumentDto> Documents { get; set; }
        public CimbResultDto Result { get; set; }
        public CimbSaleDto SaleInfo { get; set; }
        public string Creator { get; set; }
    }
}
