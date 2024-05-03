using Microsoft.AspNetCore.Mvc;
using CoreApiWithEntity.API.Resources;
using CoreApiWithEntity.BLL.Interface;
using CoreApiWithEntity.DAL;
using CoreApiWithEntity.DAL.ViewModel;

namespace CoreApiWithEntity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyAppDbContext _context;
        private readonly IUser _userRepo;
        private readonly IServices _services;
        private readonly IConfiguration _configuration;
        public UserController(MyAppDbContext context, IUser userRepo, IServices services, IConfiguration configuration)
        {
            _context = context;
            _userRepo = userRepo;
            _services = services;
            _configuration = configuration;
        }

        /// <summary>
        ///  Authenticates a user by verifying their credentials.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Userlogin([FromBody] LoginRequest request)
        {

            Response response = new Response();
            try
            {
                // Authenticate the user
                var authenticatedUser = await _userRepo.Userlogin(request);

                // Check if authentication was successful
                if (authenticatedUser != null)
                {
                    response.Data = authenticatedUser;
                    response.Sucess = true;
                    response.Message = Resource.LOGIN_SUCCESS;
                }
                else
                {
                    response.Sucess = false;
                    response.Message = Resource.LOGIN_FAILED; // You can customize this message
                }
            }
            catch (Exception ex)
            {
                response.Sucess = false;
                response.Message = ex.Message;
                string ExceptionString = "Api : Userlogin" + Environment.NewLine;
                var fileName = "Userlogin - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ex.StackTrace);
            }
            return Ok(response);
        }

        /// <summary>
        /// Registers a new entry in the system.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> UserRegistration([FromBody] UserRequest request)
        {
            Response response = new Response();
            try
            {
                response.Data = await _userRepo.UserRegistration(request);
                response.Sucess = true;
                response.Message = Resource.REGISTRATION_SUCCESS;
            }
            catch (Exception ex)
            {
                response.Sucess = false;
                response.Message = ex.Message;
                string ExceptionString = "Api : UserRegistration" + Environment.NewLine;  
                var fileName = "UserRegistration - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ex.StackTrace);
            }
            return Ok(response);
        }
    }
}
