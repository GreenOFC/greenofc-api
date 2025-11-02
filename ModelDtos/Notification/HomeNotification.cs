using System.Collections.Generic;
using _24hplusdotnetcore.ModelDtos.News;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.Notification
{
    public class HomeNotification
    {
        public long Unsecured { get; set; }
        public long News { get; set; }
        public IEnumerable<string> Banner { get; set; }
        public IEnumerable<GetNewsResponse> TopNews { get; set; }
    }
}
