using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.LeadOkVays
{
    [BsonIgnoreExtraElements]
    public class GetDetailLeadOkVayResponse
    {
        public GetDetailLeadOkVayResponse()
        {
            SaleInfo = new SaleInfoResponse();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FullName { get; set; }

        public string IdCard { get; set; }

        public string Phone { get; set; }

        public string ExtraPhone { get; set; }

        public LeadOkVayAddressDto TemporaryAddress { get; set; }

        public string Debt { get; set; }
        public string DebtId { get; set; }
        public string IncomeId { get; set; }

        public string Income { get; set; }

        public SaleInfoResponse SaleInfo { get; set; }

        public PosInfoDto PosInfo { get; set; }

        public TeamLeadDto TeamLeadInfo { get; set; }

        public LeadOkVayResultDto Result { get; set; }

        public DateTime CreatedDate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
