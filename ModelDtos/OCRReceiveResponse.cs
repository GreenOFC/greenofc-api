using _24hplusdotnetcore.Common.Attributes;

namespace _24hplusdotnetcore.ModelDtos
{
    public class OCRReceiveResponse
    {
        public string Message { get; set; }
        [Newtonsoft.Json.JsonConverter(typeof(StringTypeConverter))]
        public OCRReceiveDetailResponse JsonContent { get; set; }
    }

    public class OCRReceiveDetailResponse
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
