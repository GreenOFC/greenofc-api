using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    public class LeadOkVay: LeadSource
    {
        public string FullName { get; set; }

        public string IdCard { get; set; }

        public string Phone { get; set; }

        public string ExtraPhone { get; set; }

        public LeadOkVayAddress TemporaryAddress { get; set; }

        public string Debt { get; set; }
        public string DebtId { get; set; }
        public string IncomeId { get; set; }

        public string Income { get; set; }

        public LeadOkVayResult Result { get; set; }
    }

    public class LeadOkVayAddress
    {
        public DataConfigModel Province { get; set; }

        public string GetFullAddress()
        {
            return Province?.Value ?? string.Empty;
        }
    }

    public class LeadOkVayResult
    {
        [JsonConverter(typeof(LeadOkVayStatus))]
        [BsonRepresentation(BsonType.String)]
        public LeadOkVayStatus Status { get; private set; }

        public string Reason { get; private set; }


        public void MarkReject(string reason)
        {
            Status = LeadOkVayStatus.Reject;
            Reason = reason;
        }

        public void MarkApprove()
        {
            Status = LeadOkVayStatus.Approve;
        }
        public void MarkReview()
        {
            Status = LeadOkVayStatus.Review;
        }
    }
}
