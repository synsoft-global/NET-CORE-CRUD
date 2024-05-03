using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiWithEntity.DAL.Models
{
    public class Product
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
        public DateTime CreatedAt { get; set; }       
        public ICollection<ProductCategoryMst> ProductCategories { get; set; } = new List<ProductCategoryMst>();
    }

    public class ProductPagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
    }

    public class ProductList
    {
        public List<Product> Product { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int ProductCount { get; set; }
    }
}
