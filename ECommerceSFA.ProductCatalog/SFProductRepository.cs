﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
            IReliableDictionary<Guid, Product> products = await stateManager.
                GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");
            var result = new List<Product>();

            using (ITransaction tx = stateManager.CreateTransaction())
            {
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, Product>> allProducts = 
                    await products.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<Guid, Product>> enumerator = allProducts.GetAsyncEnumerator())
                {
                    while(await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, Product> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }

            }
            return result;
        }
    }
}
