using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    [BsonIgnoreExtraElements]
    public class LeadPtfResultDto
    {
        public string Reason { get; set; }
        public string ReturnStatus { get; set; }
        public string Note { get; set; }
        public string ContractNumber { get; set; }
    }
}
