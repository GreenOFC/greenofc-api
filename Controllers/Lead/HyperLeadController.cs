using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Lead;
using _24hplusdotnetcore.ModelDtos.Vps;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/hyper-lead")]
    public class HyperLeadController : BaseController
    {
        private readonly ILogger<HyperLeadController> _logger;
        private readonly IHyperLeadService _leadService;

        public HyperLeadController(ILogger<HyperLeadController> logger, IHyperLeadService leadService)
        {
            _logger = logger;
            _leadService = leadService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromQuery] CreateHyperLeadDto request)
        {
            try
            {
                await _leadService.CreateAsync(request);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var leadVibResponse = await _leadService.GetList(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(leadVibResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
