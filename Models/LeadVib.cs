using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.LeadSource)]
    public class LeadVib: LeadSource
    {
        public string FullName { get; set; }
        public string IdCard { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public LeadVibAddress TemporaryAddress { get; set; }
        public DataConfigModel Constitution { get; set; }
        public DataConfigModel Income { get; set; }
        public DataConfigModel Product { get; set; }

        public DateTime? GetDateOfBirth()
        {
            if (DateTime.TryParseExact(DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
    }

    public class LeadVibAddress
    {
        public DataConfigModel Province { get; set; }
        public DataConfigModel District { get; set; }
        public DataConfigModel Ward { get; set; }
        public string Street { get; set; }

        public string GetFullAddress()
        {
            return string.Join(", ", new List<string> { Street, Ward?.Value, District?.Value, Province?.Value }
                .Where(x => !string.IsNullOrEmpty(x)));
        }
    }
}
