using Newtonsoft.Json.Linq;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.EC
{
    public interface IECS3RestService
    {
        [Multipart]
        [Put("")]
        Task<JObject> PushPIDFile([AliasAs("PID")] ByteArrayPart pid);

        [Multipart]
        [Put("")]
        Task<JObject> PushPICFile([AliasAs("PIC")] ByteArrayPart pic);
    }
}
