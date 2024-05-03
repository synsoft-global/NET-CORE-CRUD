using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CoreApiWithEntity.BLL.Interface;
using CoreApiWithEntity.DAL;
using CoreApiWithEntity.DAL.Models;
using CoreApiWithEntity.DAL.Tools;
using CoreApiWithEntity.DAL.ViewModel;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoreApiWithEntity.BLL.Repository
{
    public class UserRepo : IUser
    {
        private readonly MyAppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IServices _services;
        public UserRepo(MyAppDbContext context, IConfiguration configuration, IServices services)
        {
            _context = context;          
            _configuration = configuration;
            _services = services;
        }


        public async Task<UserResponse> Userlogin([FromBody] LoginRequest request)
        {
            try
            {
                string password = Password.hashPassword(request.Password);

                // Retrieve user from the database
                var dbUser = await _context.User.Include(e => e.Roles).ThenInclude(e => e.Role).FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == password);

                if (dbUser == null)
                {
                    throw new Exception("Username or password is incorrect");
                }

                var userRoles = dbUser.Roles.Select(e => e.Role.Name).ToList();

                var token =  GenerateJwtTokenAsync(dbUser.Username, dbUser.Id, userRoles).Result;

                // Directly map properties to UserResponse
                var userResponse = new UserResponse
                {
                    Id = dbUser.Id,
                    Username = dbUser.Username,
                    Password = password,
                    Active = true,
                    Token = token,
                    Roles = userRoles    
                };

                // Return the mapped userResponse
                return userResponse;
            }
            catch (Exception ex)
            {
                string ExceptionString = "Api : Userlogin" + Environment.NewLine;
                var fileName = "Userlogin - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ex.StackTrace);

                // Rethrow the exception or handle it accordingly
                throw;
            }
        }


        public async Task<UserRequest> UserRegistration([FromBody] UserRequest request)
        {
            try
            {
                var dbUser = _context.User.Where(u => u.Username == request.Username).FirstOrDefault();

                if (dbUser != null)
                {
                    throw new Exception("Username already exists");
                }

                // Assuming you have a User entity in your DbContext
                var newUser = new User
                {
                    Username = request.Username,
                    Password = Password.hashPassword(request.Password),
                    Active = true,
                    // Map other properties as needed
                };

                _context.User.Add(newUser);
                await _context.SaveChangesAsync();

                if (request.RoleId != null && request.RoleId.Length > 0)
                {
                    var rolesToAdd = request.RoleId.Select(RoleId => new UserRole
                    {
                        RoleId = RoleId,
                        UserId = newUser.Id
                    });

                    await _context.UserRole.AddRangeAsync(rolesToAdd);
                }
                await _context.SaveChangesAsync();
                // You may want to return the registered user or some information about the registration
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                string ExceptionString = "Api : UserRegistration" + Environment.NewLine;
                var fileName = "UserRegistration - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ex.StackTrace);

                
            }
            return request;
        }

        public async Task<string> GenerateJwtTokenAsync(string username, int userId , List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);


            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, username)
            };

            // Add roles to the claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMonths(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IList<string>> GetRolesByUserIdAsync(int userId)
        {
            var userRoles = await _context.UserRole
                .Where(ur => ur.UserId == userId)
                .Join(
                    _context.Role,
                    ur => ur.RoleId,
                    role => role.Id,
                    (ur, role) => role.Name
                )
                .ToListAsync();

            return userRoles;
        }
    }
}
