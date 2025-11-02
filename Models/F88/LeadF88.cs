using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace _24hplusdotnetcore.Models.F88
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.LeadSource)]
    public class LeadF88 : LeadSource
    {
        public string F88Id { get; set; }
        public string ContractCode { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string LoanCategory { get; set; }
        public DataConfigModel LoanCategoryData { get; set; }
        public string IdCard { get; set; }
        public string IdCardProvince { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public DataConfigModel ProvinceData { get; set; }
        public string DateOfBirth { get; set; }
        public string Description { get; set; }
        public string SignAddress { get; set; }
        public DataConfigModel SignAddressData { get; set; }
        public string Status { get; set; }
        public PostBack PostBack { get; set; }

        public string GetFullAddress()
        {
            return string.Join(", ", new List<string> { Address, ProvinceData?.Value ?? Province }
                .Where(x => !string.IsNullOrEmpty(x)));
        }
        public DateTime? GetDateOfBirth()
        {
            if (DateTime.TryParseExact(DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
    }
    public class PostBack
    {
        public string Status { get; set; }
        public string DetailStatus { get; set; }
        public string LoanAmount { get; set; }
    }
}
