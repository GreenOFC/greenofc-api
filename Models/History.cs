using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.History)]
    public class History : BaseDocument
    {
        public History (string creator, string referenceId, string referenceType, AuditActionType auditActionType, string actionName, object valueBefore = null)
        {
            ReferenceId = referenceId;
            ReferenceType = referenceType;
            AuditActionType = auditActionType;
            ActionName = actionName;
            ValueBefore = valueBefore;
            Creator = creator;
        }

        public History(string creator, string referenceId, string referenceType, AuditActionType auditActionType, string actionName, object valueBefore, object valueAfter)
        {
            ReferenceId = referenceId;
            ReferenceType = referenceType;
            AuditActionType = auditActionType;
            ActionName = actionName;
            ValueBefore = valueBefore;
            ValueAfter = valueAfter;
            Creator = creator;
        }

        public string ReferenceId { get; private set; }

        public string ReferenceType { get; private set; }

        [JsonConverter(typeof(AuditActionType))]
        [BsonRepresentation(BsonType.String)]
        public AuditActionType AuditActionType { get; private set; }

        public string ActionName { get; private set; }

        public object ValueBefore { get; private set; }

        public object ValueAfter { get; private set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; private set; }

        public void SetValueAfter(object valueAfter)
        {
            ValueAfter = valueAfter;
        }
    }

    public enum AuditActionType
    {
        Create,
        Update,
        Delete
    }
}
