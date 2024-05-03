
using CoreApiWithEntity.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace CoreApiWithEntity.DAL.ViewModel
{
    public class UserResponse
    {
        public int Id { get; set; }
      
        public string Username { get; set; }
        
        public string Password { get; set; }
        public bool Active { get; set; }

        public List<string> Roles { get; set; }

        public string Token { get; set; }
    }
}
