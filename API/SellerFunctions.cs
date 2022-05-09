using System.IO;
using System.Net;
using System.Threading.Tasks;
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

namespace API
{
    public class SellerFunctions
    {
        private readonly IMapper _mapper;
        private readonly CosmosClient _client;

        public SellerFunctions(CosmosClient client)
        {
            _client = client;

            var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<StoreDTO, Store>();

                    cfg.CreateMap<UpdateStoreDTO, Seller>();

                    cfg.CreateMap<SellerDTO, Seller>();

                    cfg.CreateMap<UpdateSellerDTO, Seller>();

                    cfg.CreateMap<OrderStoreBaseDTO, OrderStoreBase>();

                    //cfg.CreateMap<IEnumerable<OrderStoreProductDTO>, IEnumerable<OrderStoreProduct>>();

                });
            _mapper = config.CreateMapper();
        }

        [FunctionName("CreateSeller")]
        [OpenApiOperation(operationId: "CreateSeller", tags: new[] { "Seller" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SellerDTO), Description = "Seller want to be created", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Seller), Description = "The OK response")]
        public async Task<IActionResult> CreateSeller(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "seller")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            SellerDTO sellerDTO = JsonConvert.DeserializeObject<SellerDTO>(requestBody);
            Seller seller = _mapper.Map<Seller>(sellerDTO);
            var sellerSvc = new SellerServices(new Repositories.SellerRepository(_client));
            var result = await sellerSvc.CreateSeller(seller);
            return new OkObjectResult(result);
        }

        [FunctionName("UpdateSeller")]
        [OpenApiOperation(operationId: "UpdateSeller", tags: new[] { "Seller"})]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateSellerDTO), Description = "Seller data want to be updated", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Seller), Description = "The OK response")]
        public async Task<IActionResult> UpdateSeller(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "seller")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            UpdateSellerDTO updateSellerDTO = JsonConvert.DeserializeObject<UpdateSellerDTO>(requestBody);
            Seller seller = _mapper.Map<Seller>(updateSellerDTO);
            var sellerSvc = new SellerServices(new Repositories.SellerRepository(_client));
            var result = await sellerSvc.UpdateSeller(seller);
            return new OkObjectResult(result);
        }

        [FunctionName("UpdateStore")]
        [OpenApiOperation(operationId: "UpdateStore", tags: new[] { "Seller" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateStoreDTO), Description = "Store data want to be updated", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Store), Description = "The OK response")]
        public async Task<IActionResult> UpdateStore(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "store")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            UpdateStoreDTO updateStoreDto = JsonConvert.DeserializeObject<UpdateStoreDTO>(requestBody);
            Seller seller = _mapper.Map<Seller>(updateStoreDto);
            var sellerSvc = new SellerServices(new Repositories.SellerRepository(_client));
            var result = await sellerSvc.UpdateStore(seller);
            return new OkObjectResult(result);
        }

        [FunctionName("CreateOrderStore")]
        [OpenApiOperation(operationId: "CreateOrderStore", tags: new[] { "Seller" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(OrderStoreBaseDTO), Description = "Order Store data want to be created", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OrderStoreBase), Description = "The OK response")]
        public async Task<IActionResult> CreateOrderStore(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "store/order")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            OrderStoreBaseDTO orderStoreDTO = JsonConvert.DeserializeObject<OrderStoreBaseDTO>(requestBody);
            OrderStoreBase seller = _mapper.Map<OrderStoreBase>(orderStoreDTO);
            var sellerSvc = new OrderStoreServices(new Repositories.OrderStoreRepository(_client));
            var result = await sellerSvc.CreateOrderStore(seller);
            return new OkObjectResult(result);
        }
    }
}

