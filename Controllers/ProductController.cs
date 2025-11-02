using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/product")]
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductServices _productServices;
        public ProductController(ILogger<ProductController> logger, ProductServices productServices)
        {
            _logger = logger;
            _productServices = productServices;
        }
        [HttpGet]
        [Route("getAll")]
        public ActionResult<ResponseContext> GetListProductByGreenType([FromQuery] string greenType, [FromQuery] string productLine)
        {
            try
            {
                var lstProduct = new List<Product>();
                lstProduct = _productServices.GetAllProduct(greenType, productLine);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = lstProduct
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
        [HttpGet]
        [Route("{productid}")]
        public ActionResult<ResponseContext> GetProductByProductId(string productid)
        {
            try
            {
                var objProduct = new Product();
                objProduct = _productServices.GetProductByProductId(productid);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = objProduct
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
        [HttpPost]
        [Route("create")]
        public ActionResult<ResponseContext> Create(Product product)
        {
            try
            {
                var newProduct = _productServices.CreateProduct(product);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = newProduct
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
        [HttpGet]
        [Route("getProductByCategoryId")]
        public ActionResult<ResponseContext> GetProductByProductCategoryId([FromQuery] string productcategoryid, [FromQuery] string productLine)
        {
            try
            {
                var lstProduct = new List<Product>();
                lstProduct = _productServices.GetProductByProductCategory(productcategoryid, productLine);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = lstProduct
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
    }
}