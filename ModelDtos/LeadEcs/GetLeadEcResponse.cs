using _24hplusdotnetcore.ModelResponses.Pos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    [BsonIgnoreExtraElements]
    public class GetLeadEcResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Status { get; set; }
        public string ContractCode { get; set; }
        public string ECRequest { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
        public LeadEcPersonalDto Personal { get; set; }
        public LeadEcAddressDto ResidentAddress { get; set; }
        public LeadEcAddressDto TemporaryAddress { get; set; }
        public GetLeadEcLoanResponse Loan { get; set; }
        public GetLeadEcWorkingResponse Working { get; set; }
        public LeadEcDisbursementInformationDto DisbursementInformation { get; set; }
        public IEnumerable<LeadEcReferenceDto> Referees { get; set; }
        public LeadEcSaleDto SaleInfo { get; set; }
        public LeadEcResultDto Result { get; set; }
        public LeadEcSelectedOfferDto LeadEcSelectedOffer { get; set; }
        public PosDetailResponse PosInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
