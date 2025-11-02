using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Dynamic;

namespace _24hplusdotnetcore.Models.PtfOmnis
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.PtfOmniMasterData)]
    public class PtfOmniMasterData: BaseDocument
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string ParentType { get; set; }

        public ExpandoObject MetaData { get; set; }

        public string ParentValue { get; set; }

        public string UniqueKey => $"{Type}-{(string.IsNullOrEmpty(Value) ? Name : Value)}";
    }
}
