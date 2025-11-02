using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.SaleChanel)]
    public class SaleChanel: BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string SaleChanelConfigUserId { get; set; }

        public SaleChanelConfigUserInfo SaleChanelConfigUserInfo { get; set; }

        public IEnumerable<SaleChanelPosInfo> Poses { get; set; }

        public SaleInfomation SaleInfo { get; set; }

        public SaleInfomation UserModified { get; set; }

        public SaleInfomation HeadOfSaleAdmin { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SaleChanelInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public SaleChanelConfigUserInfo SaleChanelConfigUserInfo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SaleChanelPosInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public PosManager Manager { get; set; }

        public SaleInfomation SaleInfo { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
