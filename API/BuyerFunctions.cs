using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using API.DTO.Buyer;
using System.Net;
using DAL.Models;
using AutoMapper;
using Microsoft.Azure.Cosmos;
using DAL;
using BLL;

namespace API
{
    public class BuyerFunctions
    {
        private readonly IMapper _mapper;
        private readonly CosmosClient _client;
        public BuyerFunctions(CosmosClient client)
        {
            _client = client;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BuyerDTO, Buyer>();
                cfg.CreateMap<OrderBuyerDTO, OrderBuyerBase>();
            });
            _mapper = config.CreateMapper();
        }
        [FunctionName("CreateBuyer")]
        [OpenApiOperation(operationId: "CreateBuyer", tags: new[] { "Buyer" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(BuyerDTO), Description = "Buyer want to be created", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Buyer), Description = "The OK response")]
        public async Task<IActionResult> CreateBuyer(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "buyer")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            BuyerDTO buyerDTO = JsonConvert.DeserializeObject<BuyerDTO>(requestBody);
            Buyer buyer = _mapper.Map<Buyer>(buyerDTO);
            var buyerSvc = new BuyerServices(new Repositories.BuyerRepository(_client));
            var result = await buyerSvc.CreateBuyer(buyer);
            return new OkObjectResult(result);
        }

        [FunctionName("CreateOrderBuyer")]
        [OpenApiOperation(operationId: "CreateOrderBuyer", tags: new[] { "Buyer" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(OrderBuyerDTO), Description = "Buyer want to be created", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OrderBuyerBase), Description = "The OK response")]
        public async Task<IActionResult> CreateOrderBuyer(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "buyer/order")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            OrderBuyerDTO orderBuyerDTO = JsonConvert.DeserializeObject<OrderBuyerDTO>(requestBody);
            OrderBuyerBase orderBuyer = _mapper.Map<OrderBuyerBase>(orderBuyerDTO);
            var orderBuyerSvc = new OrderBuyerServices(new Repositories.OrderBuyerRepository(_client));
            var result = await orderBuyerSvc.CreateOrderBuyer(orderBuyer);
            return new OkObjectResult(result);
        }
    }
}
