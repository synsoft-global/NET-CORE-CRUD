using CoreApiWithEntity.API.ViewModel;
using CoreApiWithEntity.DAL.Models;
using CoreApiWithEntity.DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiWithEntity.BLL.Interface
{
    public interface IProduct
    {
        Task AddOrUpdateProduct(ProductRequest model);
        Task <ProductList> GetAll(ProductPagination obj);
        Task<ProductDetailReponse> GetById(int id);
        Task DeleteProduct(int id);
        Task<IEnumerable<CategoryMst>> GetAllCategories();

    }
}
