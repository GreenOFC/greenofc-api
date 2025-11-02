using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    public class DataConfigModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
