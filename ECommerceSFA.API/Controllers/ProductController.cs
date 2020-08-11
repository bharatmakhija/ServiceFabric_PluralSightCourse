using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceSFA.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;



namespace ECommerceSFA.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;


        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> getProducts()
        {
            return new[] { new ApiProduct { Id = Guid.NewGuid(), Name = "fake" } };
        }


        [HttpPost]
        public async Task addProduct([FromBody] ApiProduct product)
        {
        }
    }
}