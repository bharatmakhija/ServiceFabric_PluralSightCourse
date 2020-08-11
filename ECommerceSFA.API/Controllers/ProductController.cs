using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceSFA.API.Models;
using ECommerceSFA.ProductCatalog.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;

namespace ECommerceSFA.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        /// <summary>
        /// Info: To instantiate this we will create a proxy
        /// </summary>
        private readonly IProductCatalogService _productCatalogService;


        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
            var proxyFactory = new ServiceProxyFactory(
                c => new FabricTransportServiceRemotingClientFactory());

            // Here we need to supply the remote name of the service which we want to proxy
            // pattern: fabric:/ApplicationName/ServiceName
            // In our case ApplicationName is ECommerceSFA and ServiceName is ProductCatalog
            // Another param is partition key 
            _productCatalogService = proxyFactory.CreateServiceProxy<IProductCatalogService>(
                new Uri("fabric:/ECommerceSFA/ECommerceSFA.ProductCatalog"), new ServicePartitionKey(0));
        }


        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> getProducts()
        {
            IEnumerable<Product> allProducts = await _productCatalogService.GetAllProductsAsync();

            return allProducts.Select(
                p => new ApiProduct
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Availability = p.Availability
                });
        }


        [HttpPost]
        public async Task addProduct([FromBody] ApiProduct product)
        {
            var newProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Availability = product.Availability
            };

            await _productCatalogService.AddProductAsync(newProduct);
        }
    }
}