using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/productcategory")]
    public class ProductCategoryController : BaseController
    {
        private readonly ILogger<ProductCategoryController> _logger;
        private readonly ProductCategoryServices _productCategoryServices;
        public ProductCategoryController(ILogger<ProductCategoryController> logger, ProductCategoryServices productCategoryServices)
        {
            _logger = logger;
            _productCategoryServices = productCategoryServices;
        }
        [HttpGet]
        [Route("getAll")]
        public ActionResult<ResponseContext> GetAll([FromQuery] string productLine, [FromQuery] string greenType)
        {
            var lstProductCategory = new List<ProductCategory>();
            try
            {

                lstProductCategory = _productCategoryServices.GetProductCategories(productLine, greenType);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = lstProductCategory
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR " + StatusCodes.Status500InternalServerError, message = ex.Message });
            }
        }
        [HttpGet]
        [Route("{ProductCategoryId}")]
        public ActionResult<ResponseContext> GetCategoryById(string ProductCategoryId)
        {
            dynamic objProductCategory;
            try
            {
                objProductCategory = _productCategoryServices.GetProductCategory(ProductCategoryId);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = objProductCategory
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR " + StatusCodes.Status500InternalServerError, message = ex.Message });
            }
        }
        [HttpPost]
        [Route("create")]
        public ActionResult<ResponseContext> Create(ProductCategory productCategory)
        {
            try
            {
                _productCategoryServices.Create(productCategory);
                return Ok(productCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR " + StatusCodes.Status500InternalServerError, message = ex.Message });
            }
        }
    }
}