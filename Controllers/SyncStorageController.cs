using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/v1/sync-storage")]
    public class SyncStorageController: BaseController
    {
        private readonly ISyncFileToS3Service _syncFileToS3Service;
        public SyncStorageController(ISyncFileToS3Service syncFileToS3Service)
        {
            _syncFileToS3Service = syncFileToS3Service;
        }

        [AllowAnonymous]
        [HttpGet("trigger")]
        public async Task<IActionResult> TriggerAsync()
        {
            var numberCustomerChanges = await _syncFileToS3Service.SyncAsync();
            return Ok(new { numberCustomerChanges });
        }
    }
}
