using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.F88
{
    [BsonIgnoreExtraElements]
    public class GetF88Response
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string Status { get; set; }

        public string ContractCode { get; set; }

        public string ContractId { get; set; }

        public string GreenType { get; set; }

        public string ProductLine { get; set; }

        public F88PersonalDto Personal { get; set; }

        public F88LoanDto Loan { get; set; }

        public SaleDto SaleInfo { get; set; }

        public ResultDto Result { get; set; }

        public PosInfoDto PosInfo { get; set; }

        public TeamLeadDto TeamLeadInfo { get; set; }

        public SaleChanelDto SaleChanelInfo { get; set; }
    }
}
