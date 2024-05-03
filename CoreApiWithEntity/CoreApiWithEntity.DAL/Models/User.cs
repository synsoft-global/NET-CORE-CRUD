using CoreApiWithEntity.DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiWithEntity.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Active { get; set; }

        public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
    }
   
}
