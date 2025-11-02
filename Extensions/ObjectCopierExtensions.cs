using Newtonsoft.Json;

namespace _24hplusdotnetcore.Extensions
{
    public static class ObjectCopierExtensions
    {
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
