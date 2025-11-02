using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _24hplusdotnetcore.Controllers
{
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
    }
}
