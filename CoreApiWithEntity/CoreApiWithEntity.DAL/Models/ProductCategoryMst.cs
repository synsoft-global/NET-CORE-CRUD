using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiWithEntity.DAL.Models
{
    public class ProductCategoryMst
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public Product Product { get; set; }

        public CategoryMst Category { get; set; }

    }
}
