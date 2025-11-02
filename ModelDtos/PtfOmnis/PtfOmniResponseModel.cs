using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniResponseModel<T>
    {
        public bool Success { get; set; }
        public PtfOmniResponseErrorModel Error { get; set; }
        public T Data { get; set; }

        public string GetErrorMessage()
        {
            string msg = "Lỗi không xác định";
            if (Error != null)
            {
                if (Error.Errors?.Any() == true)
                {
                    msg = JsonConvert.SerializeObject(Error.Errors);
                }
                else
                {
                    msg = Error.Message;
                }
            }
            return msg;
        }
    }

    public class PtfOmniResponseErrorModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public IEnumerable<object> Errors { get; set; }
    }

    public class PtfOmniResponseDataModel<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
