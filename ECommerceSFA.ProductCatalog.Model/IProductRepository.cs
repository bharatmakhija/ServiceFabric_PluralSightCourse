using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSFA.ProductCatalog.Model
{
    public interface IProductRepository
    {

       public Task<IEnumerable<Product>> GetAllProducts();

        public Task AddProduct(Product product);
    }
}
