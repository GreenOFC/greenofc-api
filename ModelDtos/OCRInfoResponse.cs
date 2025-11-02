using System;

namespace _24hplusdotnetcore.ModelDtos
{
    public class OCRInfoResponse
    {
        public string Id { get; set; }
        public DateTime? Createdtime { get; set; }
        public OCRResponseModel Response { get; set; }
        public string Status { get; set; }
        public DateTime? Modifiedtime { get; set; }
        public OCRResultDetailResponse Result { get; set; }
    }

    public class OCRResponseModel
    {
        public string TypeName { get; set; }
        public string FileName { get; set; }
        public string KeyImages { get; set; }
    }

    public class OCRResultDetailResponse
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
