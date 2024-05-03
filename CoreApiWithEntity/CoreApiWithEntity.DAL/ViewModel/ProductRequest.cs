using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CoreApiWithEntity.API.ViewModel
{
    public class ProductRequest
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string ProductName { get; set; }
        [Required, MaxLength(200)]
        public string ProductDetail { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Qty { get; set; }

        public int[] CategoryIds { get; set; }
       
    }
}
