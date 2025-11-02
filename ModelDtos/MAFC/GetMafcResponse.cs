using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.ModelResponses.Pos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    [BsonIgnoreExtraElements]
    public class GetMafcResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ContractCode { get; set; }
        public int MAFCId { get; set; }
        public string GreenType { get; set; }
        public string ProductLine { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public MafcPersonalDto Personal { get; set; }
        public MafcLoanDto Loan { get; set; }
        public MafcResultDto Result { get; set; }
        public PosDetailResponse PosInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public SaleDto SaleInfo { get; set; }
        public TeamLeadDto TeamLeadInfo { get; set; }
        public SaleChanelDto SaleChanelInfo { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class GetOldMafcResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ContractCode { get; set; }
        public int MAFCId { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
    }
}
