using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CoreApiWithEntity.API.ViewModel;
using CoreApiWithEntity.BLL.Interface;
using CoreApiWithEntity.DAL;
using CoreApiWithEntity.DAL.Models;
using CoreApiWithEntity.DAL.ViewModel;

namespace CoreApiWithEntity.BLL.Repository
{
    public class ProductRepo : IProduct
    {
        private readonly MyAppDbContext _context;
        private readonly IServices _services;
        private readonly IConfiguration _configuration;

        public ProductRepo(MyAppDbContext context, IServices services, IConfiguration configuration)
        {
            _context = context;
            _services = services;
            _configuration = configuration;
        }


        public async Task<ProductList> GetAll(ProductPagination obj)
        {

            try
            {
                var Product = await _context.Product.ToListAsync();

                var query = Product.Where(e => e.ProductName.Contains(obj.SearchText));

                if (obj.SortOrder.ToUpper() == "DESC") 
                {

                    switch (obj.SortColumn)
                    {
                        case "Id":
                            query = query.OrderByDescending(e => e.Id);
                            break;

                        case "ProductName":
                            query = query.OrderByDescending(e => e.ProductName);
                            break;

                        case "ProductDetail":
                            query = query.OrderByDescending(e => e.ProductDetail);
                            break;

                        case "Price":
                            query = query.OrderByDescending(e => e.Price);
                            break;
                    }
                }
                else
                {
                    switch (obj.SortColumn)
                    {
                        case "Id":
                            query = query.OrderBy(e => e.Id);
                            break;

                        case "ProductName":
                            query = query.OrderBy(e => e.ProductName);
                            break;

                        case "ProductDetail":
                            query = query.OrderBy(e => e.ProductDetail);
                            break;

                        case "Price":
                            query = query.OrderBy(e => e.Price);
                            break;
                    }
                }

                int totalCount = Product.Count(); // total count of the Product

                var offsets = (obj.PageNumber - 1) * obj.PageSize;
                var query1 = query.Skip(offsets).Take(obj.PageSize);

                var productlist = new ProductList
                {
                    Product = query1.ToList(),
                    PageSize = obj.PageSize,
                    TotalCount = totalCount,
                    ProductCount = query.Count()
                };
                return productlist;     
            }
            catch (Exception ex)
            {               
                string ExceptionString = "Api : GetAll" + Environment.NewLine;
                var fileName = "GetAll - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ex.StackTrace);

                throw;
            }
        }

        public async Task<ProductDetailReponse> GetById(int id)
        {// Linq Query
         //List<Product> Product = new List<Product>();
            var Product = await _context.Product.Where(a => a.Id == id)
            .Select(p => new ProductDetailReponse
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ProductDetail = p.ProductDetail,
                Price = p.Price,
                Qty = p.Qty,
                CreatedAt = p.CreatedAt,
                Categories = p.ProductCategories.Where(e => e.ProductId == p.Id).Select(e => e.Category).ToList()
            })
            .FirstOrDefaultAsync();
            return Product;
        }

        //public async Task AddProduct(ProductRequest model)
        //{
        //    Product product = new Product
        //    {
        //       Id = model.Id,
        //       ProductName = model.ProductName,
        //       ProductDetail = model.ProductDetail,
        //       Price = model.Price,
        //       Qty = model.Qty,
        //       CreatedAt = model.CreatedAt
        //    };
        //    await _context.Product.AddAsync(product);
        //    _context.SaveChanges();

        //    if (model.CategoryIds != null && model.CategoryIds.Length > 0)
        //    {
        //        foreach (var categoryId in model.CategoryIds)
        //        {

        //            var productCategory = new ProductCategoryMst
        //            {
        //                CategoryId = categoryId,
        //                ProductId = model.Id
        //            };
        //            await _context.ProductCategoryMst.AddAsync(productCategory);
        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task UpdateProduct(Product model)
        //{
        //     _context.Product.Update(model);
        //      await _context.SaveChangesAsync();
        //}

        public async Task AddOrUpdateProduct(ProductRequest model)
        {
            try
            {
                var existingProduct = await _context.Product.FirstOrDefaultAsync(e => e.Id == model.Id);

                if (existingProduct != null)
                {
                    // Update existing product
                    existingProduct.ProductName = model.ProductName;
                    existingProduct.ProductDetail = model.ProductDetail;
                    existingProduct.Price = model.Price;
                    existingProduct.Qty = model.Qty;

                    // Update associated categories
                    var existingCategories = await _context.ProductCategoryMst
                        .Where(pc => pc.ProductId == model.Id)
                        .ToListAsync();

                    // Remove existing categories not present in the updated model
                    var categoriesToRemove = existingCategories
                        .Where(ec => !model.CategoryIds.Contains(ec.CategoryId))
                        .ToList();

                    _context.ProductCategoryMst.RemoveRange(categoriesToRemove);

                    // Add new categories
                    var categoriesToAdd = model.CategoryIds
                        .Where(categoryId => !existingCategories.Any(ec => ec.CategoryId == categoryId))
                        .Select(categoryId => new ProductCategoryMst
                        {
                            CategoryId = categoryId,
                            ProductId = model.Id
                        });

                    await _context.ProductCategoryMst.AddRangeAsync(categoriesToAdd);
                }
                else
                {
                    // Add new product
                    var newProduct = new Product
                    {
                        ProductName = model.ProductName,
                        ProductDetail = model.ProductDetail,
                        Price = model.Price,
                        Qty = model.Qty,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _context.Product.AddAsync(newProduct);
                    await _context.SaveChangesAsync();
                    // Add associated categories
                    if (model.CategoryIds != null && model.CategoryIds.Length > 0)
                    {
                        var categoriesToAdd = model.CategoryIds.Select(categoryId => new ProductCategoryMst
                        {
                            CategoryId = categoryId,
                            ProductId = newProduct.Id
                        });

                        await _context.ProductCategoryMst.AddRangeAsync(categoriesToAdd);
                    }
                }

                // Save changes
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                string ExceptionString = "Api : AddOrUpdateProduct" + Environment.NewLine;
                var fileName = "AddOrUpdateProduct - " + System.DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss");
                _services.SendMail(_configuration["Log:ErroAddress"], fileName, ex.StackTrace);
            }
        }

        public async Task DeleteProduct(int id)
        {
            // Find the product to delete
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            // Remove product from the ProductCategory table
            var productCategories = _context.ProductCategoryMst.Where(pc => pc.ProductId == id);
            _context.ProductCategoryMst.RemoveRange(productCategories);

            // Remove product from the Product table
            _context.Product.Remove(product);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryMst>> GetAllCategories()
        {
            var categories = await _context.CategoryMst.ToListAsync();
            return categories;
        }

    }
}
