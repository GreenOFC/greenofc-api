using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    public class OCR
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public OCRPayLoad PayLoad { get; set; }
        public OCRResponse Response { get; set; }

        public string Status { get; set; } = OCRStatus.INPROGRESS;
        public int RetryCount { get; set; } = 0;
        public OCRResult Result { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Createdtime { get; set; } = DateTime.Now;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Modifiedtime { get; set; }
    }

    public class OCRPayLoad
    {
        public string Type { get; set; }
        public IEnumerable<OCRFile> Files { get; set; }
    }

    public class OCRFile
    {
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public string FileName { get; set; }
    }

    public class OCRResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string TypeName { get; set; }
        public string FileName { get; set; }
        public string KeyImages { get; set; }
    }

    public class OCRResult
    {
        public string Message { get; set; }
        public OCRResultDetail Detail { get; set; }
    }

    public class OCRResultDetail
    {
        public string Fullname { get; set; }
        public string Dayofbrith { get; set; }
        public string Address { get; set; }
        public string Dateofissue { get; set; }
        public string Placeofisssue { get; set; }
        public string Nationality { get; set; }
        public string Dateofexpiry { get; set; }
        public string Permanentaddress { get; set; }
        public string Number { get; set; }
        public string Idcardofmain { get; set; }
        public string Idcard { get; set; }
        public string Mobiles { get; set; }
    }
}
