using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommerceSFA.ProductCatalog.Model;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ECommerceSFA.ProductCatalog
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ProductCatalog : StatefulService, IProductCatalogService
    {
        private IProductRepository productRepository;

        public ProductCatalog(StatefulServiceContext context)
            : base(context)
        { }

        public async Task AddProductAsync(Product product)
        {
            await productRepository.AddProduct(product);
        }

        public async Task<Product[]> GetAllProductsAsync()
        {
           return (await productRepository.GetAllProducts()).ToArray();
            /// Here IEnumerable is being converted to array 
            /// Reason: ServiceFabricRemoting doesn't understand IEnumerable, as mentioned in the course.
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(
                    context => new FabricTransportServiceRemotingListener(context, this))
            };
            /// FabricTransportServiceRemotingListener is a standard class exposes service remoting
            /// It allows you to plug in your own server implementation
            /// So after this productCatalog is exposing methods to other microservices.
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            productRepository = new SFProductRepository(this.StateManager);

            var prod1 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Description = "asdad",
                Price = 20,
                Availability = "12"
            };

            await productRepository.AddProduct(prod1);
            Console.WriteLine("added product");

            IEnumerable<Product> allProducts = await productRepository.GetAllProducts();

            Console.WriteLine("all products are retrivable");
        }
    }
}
