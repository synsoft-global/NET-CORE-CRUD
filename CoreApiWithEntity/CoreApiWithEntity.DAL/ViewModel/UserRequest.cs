using System.ComponentModel.DataAnnotations;


namespace CoreApiWithEntity.DAL.ViewModel
{
    public class UserRequest
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Active { get; set; }

        public int[] RoleId { get; set; }
    }
}
