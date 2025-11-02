using System;
using System.Collections.Generic;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadVibs
{
    public class GetLeadVibResponse : LeadVibDto
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public SaleInfomationDto SaleInfomation { private get; set; }
        public PosInfoDto PosInfo { get; set; }
        public TeamLeadDto TeamLeadInfo { get; set; }
        public SaleChanelDto SaleChanelInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
