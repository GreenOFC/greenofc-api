
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos.Shinhan;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.Shinhan;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/shinhan")]
    public class ShinhanController : BaseController
    {
        private readonly ILogger<ShinhanController> _logger;
        private readonly UserServices _userServices;
        private readonly IShinhanApplicationService _shinhanApplicationService;
        private readonly IMapper _mapper;

        public ShinhanController(
            ILogger<ShinhanController> logger,
            UserServices userServices,
            IShinhanApplicationService shinhanApplicationService,
            IMapper mapper
        )
        {
            _logger = logger;
            _userServices = userServices;
            _shinhanApplicationService = shinhanApplicationService;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAsync([FromQuery] GetShinhanResquest getShinhanResquest)
        {
            try
            {
                var result = await _shinhanApplicationService.GetAsync(getShinhanResquest);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [CheckUserApprovedAuthotization]
        [HttpPost("web/step-1")]
        public async Task<IActionResult> CreateAsync(CreateShinhanRequest createShinhanRequest)
        {
            try
            {
                var response = await _shinhanApplicationService.CreateAsync(createShinhanRequest);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("web/{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateShinhanRequest updateShinhanRequest)
        {
            try
            {
                await _shinhanApplicationService.UpdateAsync(id, updateShinhanRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("mobile/step-1")]
        public async Task<IActionResult> CreateAsync(CreateShinhanStep1Request createShinhanRequest)
        {
            try
            {
                var response = await _shinhanApplicationService.CreateStep1Async(createShinhanRequest);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("mobile/{id}/step-1")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateShinhanStep1Request updateShinhanRequest)
        {
            try
            {
                await _shinhanApplicationService.UpdateStep1Async(id, updateShinhanRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("mobile/{id}/step-2")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateShinhanStep2Request updateShinhanRequest)
        {
            try
            {
                await _shinhanApplicationService.UpdateStep2Async(id, updateShinhanRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("mobile/{id}/step-3")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateShinhanStep3Request updateShinhanRequest)
        {
            try
            {
                await _shinhanApplicationService.UpdateStep3Async(id, updateShinhanRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("mobile/{id}/step-4")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateShinhanStep4Request updateShinhanRequest)
        {
            try
            {
                // var listProvinceIds = new List<string>() {
                //     "42", // HỒ CHÍ MINH
                //     "1", // HÀ NỘI
                //     "4", // HƯNG YÊN
                //     "32", // ĐÀ NẴNG
                //     "47", // BÌNH DƯƠNG
                //     "3", // HẢI DƯƠNG
                //     "20", // BẮC NINH
                //     "48", // ĐỒNG NAI
                //     "51", // LONG AN
                //     "50", // BÀ RỊA VŨNG TÀU
                //     "33", // QUẢNG NAM
                //     "18", // VĨNH PHÚC
                //     "37", // KHÁNH HÒA
                //     "34", // QUÃNG NGÃI
                //     "28", // CẦN THƠ
                //     "55", // VĨNH LONG
                //     "59", // HẬU GIANG
                //     "35", // BÌNH ĐỊNH
                //     "16", // THÁI NGUYÊN
                //     "19", // BẮC GIANG
                //     "26", // THANH HÓA
                //     "6", // NAM ĐỊNH
                //     "5", // HÀ NAM
                //     "7", // THÁI BÌNH
                //     "17", // PHÚ THỌ
                //     "31", // THỪA THIÊN HUẾ
                //     "54", // TIỀN GIANG
                //     "56", // BẾN TRE
                //     "53", // AN GIANG
                //     "52", // ĐỒNG THÁP
                // };
                // if (listProvinceIds.Where(x => x == updateShinhanRequest.TemporaryAddress.ProvinceId).Any())
                // {
                //     await _shinhanApplicationService.UpdateStep4Async(id, updateShinhanRequest);
                //     return Ok(ResponseContext.GetSuccessInstance());
                // }
                // else
                // {
                //     return Ok(ResponseContext.GetErrorInstance("Khu vực không hỗ trợ vay vốn"));
                // }
                await _shinhanApplicationService.UpdateStep4Async(id, updateShinhanRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("mobile/{id}/step-5")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateShinhanStep5Request updateShinhanDocumentRequest)
        {
            try
            {
                await _shinhanApplicationService.UpdateStep5Async(id, updateShinhanDocumentRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var result = await _shinhanApplicationService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}