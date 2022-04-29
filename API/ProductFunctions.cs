using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using API.DTO.Product;
using API.DTO.Seller;
using AutoMapper;
using BLL;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API
{
    public class ProductFunctions
    {
        private readonly IMapper _mapper;
        private readonly CosmosClient _client;

        public ProductFunctions(CosmosClient client)
        {
            _client = client;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDTO, Product>();
                cfg.CreateMap<Product, ProductDTO>();

                cfg.CreateMap<StoreDTO, Store>();
                cfg.CreateMap<Store, StoreDTO>();

                cfg.CreateMap<UpdateProductDTO, Product>();
                cfg.CreateMap<Product, UpdateProductDTO>();

                cfg.CreateMap<DeleteProductDTO, Product>();
                cfg.CreateMap<Product, DeleteProductDTO>();
            });
            _mapper = config.CreateMapper();
        }

        [FunctionName("CreateProduct")]
        [OpenApiOperation(operationId: "CreateProduct", tags: new[] { "Product" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ProductDTO), Description = "Product want to be created", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product), Description = "The OK response")]
        public async Task<IActionResult> CreateProduct(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "product")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
            Product product = _mapper.Map<Product>(productDTO);
            var productSvc = new ProductServices(new Repositories.ProductRepository(_client));
            var result = await productSvc.CreateProduct(product);
            return new OkObjectResult(result);
        }

        [FunctionName("GetAllProduct")]
        [OpenApiOperation(operationId: "GetAllProduct", tags: new[] { "Product" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Product>), Description = "The OK response")]
        public async Task<IActionResult> GetAllProduct(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "product")] HttpRequest req,
    ILogger log)
        {
            var productSvc = new ProductServices(new Repositories.ProductRepository(_client));
            var result = await productSvc.GetAllProduct();
            return new OkObjectResult(result);
        }

        [FunctionName("UpdateProduct")]
        [OpenApiOperation(operationId: "UpdateProduct", tags: new[] { "Product" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateProductDTO), Description = "Product data want to be updated", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product), Description = "The OK response")]
        public async Task<IActionResult> UpdateProduct(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "product")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            UpdateProductDTO updateProductDTO = JsonConvert.DeserializeObject<UpdateProductDTO>(requestBody);
            Product product = _mapper.Map<Product>(updateProductDTO);
            var productSvc = new ProductServices(new Repositories.ProductRepository(_client));
            var result = await productSvc.UpdateProduct(product);
            return new OkObjectResult(result);
        }

        [FunctionName("DeleteProduct")]
        [OpenApiOperation(operationId: "DeleteProduct", tags: new[] { "Product" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DeleteProductDTO), Description = "Product data want to be updated", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> DeleteProduct(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "product")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            DeleteProductDTO deleteProductDTO = JsonConvert.DeserializeObject<DeleteProductDTO>(requestBody);
            Product product = _mapper.Map<Product>(deleteProductDTO);
            var productSvc = new ProductServices(new Repositories.ProductRepository(_client));
            await productSvc.DeleteProduct(product);
            return new OkObjectResult($"{product.Id} has been deleted");
        }
    }
}

