using System;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.Models
{
    [Serializable]
    public class ResponseContext
    {
        public int code { get; set; }
        public string message { get; set; }
        public dynamic data { get; set; }

        public static ResponseContext GetSuccessInstance(dynamic data = null)
        {
            return new ResponseContext
            {
                code = (int)Common.ResponseCode.SUCCESS,
                message = Common.Message.SUCCESS,
                data = data
            };
        }
        public static ResponseContext GetErrorInstance(string message, dynamic data = null)
        {
            return new ResponseContext
            {
                code = (int)Common.ResponseCode.ERROR,
                message = message,
                data = data
            };
        }
    }
    [Serializable]
    public class ResponseMAFCContext
    {
        public string status { get; set; }
        public string message { get; set; }
        public dynamic data { get; set; }
    }

    public class ResponseMAContext
    {
        [JsonProperty("Result")]
        public bool Result { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
    }
    public class ResponseMCContext
    {
        [JsonProperty("returnCode")]
        public string ReturnCode { get; set; }
        [JsonProperty("returnMes")]
        public string ReturnMes { get; set; }
    }
}