using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace ECommerceSFA.ProductCatalog.Model
{
    /// <summary>
    /// IService implementation indicates service fabric that this interface will be used for service remoting.
    /// ProductCatalog will implement IProductCatalogService
    /// </summary>
    public interface IProductCatalogService : IService
    {
        Task<Product[]> GetAllProductsAsync();

        Task AddProductAsync(Product product);
    }
}
