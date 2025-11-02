using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    [BsonIgnoreExtraElements]
    public class PtfOmniGetDataProcessingResponse
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string Step { get; set; }
        public IEnumerable<PtfOmniGetDataProcessingPayloadDto> Payloads { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class PtfOmniGetDataProcessingPayloadDto
    {
        public string Message { get; set; }
        public string Payload { get; set; }
        public string Response { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
