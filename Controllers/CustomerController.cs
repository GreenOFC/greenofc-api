using _24hplusdotnetcore.BatchJob;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/customer")]
    public class CustomerController : BaseController
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly CustomerServices _customerServices;
        private readonly FileUploadServices _fileUploadServices;
        private readonly MobileVersionServices _mobileVersionServices;
        private readonly ProductServices _productServices;
        private readonly IMapper _mapper;

        public CustomerController(ILogger<CustomerController> logger,
                                CustomerServices customerServices,
                                FileUploadServices fileUploadServices,
                                MobileVersionServices mobileVersionServices,
                                IMapper mapper,
                                ProductServices productServices)
        {
            _logger = logger;
            _customerServices = customerServices;
            _fileUploadServices = fileUploadServices;
            _mobileVersionServices = mobileVersionServices;
            _productServices = productServices;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getAll")]
        public ActionResult<ResponseContext> GetCustomerList([FromQuery] string productLine, [FromQuery] string greenType, [FromQuery] string status, 
            [FromQuery] string customername, [FromQuery] string fromDate, [FromQuery] string toDate, [FromQuery] int? pagenumber, [FromQuery] int? pagesize,
            [FromQuery] string teamLead, [FromQuery] string posManager, [FromQuery] string sale)
        {
            try
            {

                var user = (User)HttpContext.Items["User"];
                var lstCustomers = new List<Customer>();
                long totalPage = 0;
                long totalrecord = 0;
                if (user.RoleName == "Report" ||
                    ((user.RoleName == "ViewMAFC" || user.RoleName == "SaleAdminMAFC") && greenType == "A") ||
                    ((user.RoleName == "ViewMC" || user.RoleName == "SaleAdminMAFC") && greenType == "C") ||
                    ((user.RoleName == "ViewSIN") && greenType == "E")
                    )
                {
                    lstCustomers = _customerServices.GetAllList(greenType, productLine, status, customername, fromDate, toDate, pagenumber, pagesize, 
                        ref totalPage, ref totalrecord, teamLead, posManager, sale);
                }
                else
                {
                    lstCustomers = _customerServices.GetList(user, greenType, productLine, status, customername, fromDate, toDate, pagenumber, pagesize, 
                        ref totalPage, ref totalrecord, teamLead, posManager, sale);
                }

                var lstCustomerOptimization = _mapper.Map<IEnumerable<CustomerResponseModel>>(lstCustomers);


                var datasizeInfo = _customerServices.CustomerPagesize(lstCustomers);
                return Ok(new PagingDataResponse
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = lstCustomerOptimization,
                    pagenumber = pagenumber.HasValue ? (int)pagenumber : 1,
                    totalpage = totalPage,
                    totalrecord = totalrecord
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult<ResponseContext> GetCustomerList()
        {
            try
            {

                var lstCustomers = _customerServices.GetList("A", "");

                var lstCustomerOptimization = _mapper.Map<IEnumerable<CustomerResponseModel>>(lstCustomers);

                return Ok(ResponseContext.GetSuccessInstance(lstCustomerOptimization));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
        [HttpGet]
        [Route("countstatus")]
        public ActionResult<ResponseContext> CustomerSatusCount([FromQuery] string greenType, [FromQuery] string productLine)
        {
            try
            {
                var user = (User)HttpContext.Items["User"];
                var statusCount = _customerServices.GetStatusCount(user, greenType, productLine);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = statusCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public ActionResult<ResponseContext> GetCustomer(string id)
        {
            try
            {
                Customer result = _customerServices.GetCustomer(id);
                if (result != null)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result));
                }
                else
                {
                    return Ok(ResponseContext.GetErrorInstance(Common.Message.CUSTOMER_NOT_FOUND));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }


    }
}