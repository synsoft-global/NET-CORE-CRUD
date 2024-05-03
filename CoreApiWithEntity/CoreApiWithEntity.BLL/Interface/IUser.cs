using Microsoft.AspNetCore.Mvc;
using CoreApiWithEntity.DAL.Models;
using CoreApiWithEntity.DAL.ViewModel;

namespace CoreApiWithEntity.BLL.Interface
{
    public interface IUser
    {
        Task<UserResponse> Userlogin([FromBody] LoginRequest request);
        Task<UserRequest> UserRegistration([FromBody] UserRequest request);
        Task<IList<string>> GetRolesByUserIdAsync(int userId);
        Task<string> GenerateJwtTokenAsync(string username, int userId, List<string> roles);
    }
}
