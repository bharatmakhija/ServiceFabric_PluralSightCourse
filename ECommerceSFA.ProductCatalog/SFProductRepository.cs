using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ECommerceSFA.ProductCatalog.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace ECommerceSFA.ProductCatalog
{
    class SFProductRepository : IProductRepository
    {
        private readonly IReliableStateManager stateManager;

        public SFProductRepository(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task AddProduct(Product product)
        {
            IReliableDictionary<Guid, Product> products = await stateManager
                .GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");

            using(ITransaction tx = stateManager.CreateTransaction())
            {

                await products.AddOrUpdateAsync(tx, product.Id, product, (id, value) => product);
                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            throw new NotImplementedException();
        }
    }
}
