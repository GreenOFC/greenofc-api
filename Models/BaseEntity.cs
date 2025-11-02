using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.Models
{
    public interface IBaseDocument
    {
        string Id { get; set; }
    }
    public interface IBaseEntity : IBaseDocument
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
    }

    public class BaseDocument : IBaseDocument
    {
        [DisableAuditing]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [DisableAuditing]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DisableAuditing]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }

    public abstract class BaseEntity : IBaseEntity, ISoftDelete
    {
        [DisableAuditing]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [DisableAuditing]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }

        [DisableAuditing]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Modifier { get; set; }

        [DisableAuditing]
        public bool IsDeleted { get; set; }

        [DisableAuditing]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeletedBy { get; set; }

        [DisableAuditing]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DisableAuditing]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        [DisableAuditing]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DeletedDate { get; set; }
        
    }


    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
