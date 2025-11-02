using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    [BsonIgnoreExtraElements]
    public class HistoryCallApiLogResponse
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string Action { get; set; }
        public string RemoteIpAddress { get; set; }

        public string AbsolutePath { get; set; }

        public string Payload { get; set; }

        public string Response { get; set; }

        public string Message { get; set; }

        public SaleInfomationDto SaleInfo { get; set; }
        public TeamLeadDto TeamLeadInfo { get; set; }
        public TeamLeadDto AsmInfo { get; set; }
        public PosInfoDto PosInfo { get; set; }
        public SaleChanelDto SaleChanelInfo { get; set; }

    }
}
