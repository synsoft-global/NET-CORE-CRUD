using FluentAssertions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using CoreApiWithEntity.API.Resources;
using CoreApiWithEntity.API.ViewModel;
using CoreApiWithEntity.BLL.Interface;
using CoreApiWithEntity.DAL;
using CoreApiWithEntity.DAL.Models;

namespace CoreApiWithEntity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productRepo;
        private readonly IServices _services;
        private readonly IConfiguration _configuration;

        public ProductController(IProduct productRepo, IServices services, IConfiguration configuration)
        {
           
            _productRepo = productRepo;           
            _services = services;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Get product details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("GetProduct")]
        [Authorize(Roles="User, Admin")]
        public async Task<Response> GetProduct(ProductPagination obj)
        {
            Response response = new Response();
            try
            {
                response.Data = await _productRepo.GetAll(obj);
                response.Sucess = true;
                response.Message = Resource.SUCCESS;
            }
            catch(Exception ex)
            {
                response.Sucess = false;
                response.Message = ex.Message;
                var fileName = "GetProduct - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ex.StackTrace);
            }
            return response;
        }


        /// <summary>
        /// Get product by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [Authorize]
        public async Task<Response> GetProductById(int Id)
        {
            Response response = new Response();
            try
            {
                response.Data = await _productRepo.GetById(Id);
                response.Sucess = true;
                response.Message = Resource.SUCCESS;
            }
            catch(Exception ex)
            {
                response.Sucess = false;
                response.Message = ex.Message;
                string ExceptionString = "Api : GetProductById" + Environment.NewLine;
                ExceptionString += "Message : "+ ex.Message + "Exception : " + JsonConvert.SerializeObject(Response) + Environment.NewLine;
                var fileName = "GetProductById - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ExceptionString);
            }
            return response;
        }

        /// <summary>
        /// Add and update products
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task AddOrUpdateProduct(ProductRequest model)
        {
            Response response = new Response();
            try
            {
                await _productRepo.AddOrUpdateProduct(model);
                response.Sucess = true;
                response.Message = model.Id>0 ? Resource.UPDATE_PRODUCT : Resource.ADD_PRODUCT;
            }
            catch(Exception ex)
            {
                response.Sucess = false;
                response.Message = ex.Message;
                string ExceptionString = "Api : AddOrUpdateProduct" + Environment.NewLine;
                var fileName = "AddOrUpdateProduct - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ex.StackTrace);
            }
        }

        /// <summary>
        /// delete product details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [Authorize]
        public async Task DeleteProduct(int Id)
        {
            Response response = new Response();
            try
            {
                await _productRepo.DeleteProduct(Id);
                response.Sucess = true;
                response.Message = Resource.DELETE_PRODUCT;
            }
            catch(Exception ex)
            {
                response.Sucess = false;
                response.Message = ex.Message;
                string ExceptionString = "Api : DeleteProduct" + Environment.NewLine;
                var fileName = "DeleteProduct - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ex.StackTrace);
            }
        }

        /// <summary>
        /// Get all Product Categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetCategories")]
        public async Task<Response> GetCategories()
        {
            Response response = new Response();
            try
            {
                response.Data = await _productRepo.GetAllCategories();
                response.Sucess = true;
                response.Message = Resource.SUCCESS;
            }
            catch(Exception ex)
            {
                response.Sucess= false;
                response.Message = ex.Message;
                string ExceptionString = "Api : GetCategories" + Environment.NewLine;
                var fileName = "GetCategories - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ex.StackTrace);
            }
            return response;
        }
    }
}
