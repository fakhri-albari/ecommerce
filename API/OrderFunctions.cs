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
using System.Net;
using API.DTO.Order;
using AutoMapper;
using Microsoft.Azure.Cosmos;
using DAL.Models;
using DAL;
using BLL;
using System.Net.Http;

namespace API
{
    public class OrderFunctions
    {
        private readonly IMapper _mapper;
        private readonly CosmosClient _client;

        public OrderFunctions(CosmosClient client)
        {
            _client = client;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, Order>();
                cfg.CreateMap<OrderPaymentDTO, OrderPayment>();
                cfg.CreateMap<UpdateOrderPaymentDTO, OrderPayment>();
                cfg.CreateMap<OrderStoreDTO, OrderStore>();
                cfg.CreateMap<OrderProductDTO, OrderProduct>();
                cfg.CreateMap<PaymentDetailStatus ,OrderPaymentDetailStatus >();
            });
            _mapper = config.CreateMapper();
        }

        [FunctionName("CreateOrder")]
        [OpenApiOperation(operationId: "CreateOrder", tags: new[] { "Order" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(OrderDTO), Description = "Order want to be created", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "The OK response")]
        public async Task<IActionResult> CreateOrder(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "order")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            Order order = _mapper.Map<Order>(orderDTO);
            var orderSvc = new OrderServices(new Repositories.OrderRepository(_client));
            var result = await orderSvc.CreateOrder(order);
            log.LogWarning(" ");
            log.LogWarning("=== Order Created ===");
            log.LogWarning($"ID: {result.Id}");
            log.LogWarning(" ");
            return new OkObjectResult(result);
        }

        [FunctionName("UpdateOrderPayment")]
        [OpenApiOperation(operationId: "UpdateOrderPayment", tags: new[] { "Order" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateOrderPaymentDTO), Description = "Order payment data want to be updated", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "The OK response")]
        public async Task<IActionResult> UpdateOrderPayment(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "order/payment")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            UpdateOrderPaymentDTO updateOrderPaymentDTO = JsonConvert.DeserializeObject<UpdateOrderPaymentDTO>(requestBody);
            OrderPayment orderPayment = new OrderPayment
            {
                method = updateOrderPaymentDTO.method,
                status = updateOrderPaymentDTO.status,
                id = updateOrderPaymentDTO.id,
                total = updateOrderPaymentDTO.total,
                detailStatus = _mapper.Map<OrderPaymentDetailStatus>(updateOrderPaymentDTO.detailStatus)
            };
            var orderSvc = new OrderServices(new Repositories.OrderRepository(_client));
            var result = await orderSvc.UpdateOrderPayment(orderPayment, updateOrderPaymentDTO.orderId);
            log.LogWarning(" ");
            log.LogWarning("=== Order Payment Updated ===");
            log.LogWarning($"Order ID      : {result.Id}");
            log.LogWarning($"Payment Status: {result.payment.status}");
            log.LogWarning(" ");
            return new OkObjectResult(result);
        }

        [FunctionName("UpdateOrderStatus")]
        [OpenApiOperation(operationId: "UpdateOrderStatus", tags: new[] { "Order" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateOrderStatus), Description = "Order Status data want to be updated", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "The OK response")]
        public async Task<IActionResult> UpdateOrderStatus(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "order/status")] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            UpdateOrderStatus updateOrderStatus = JsonConvert.DeserializeObject<UpdateOrderStatus>(requestBody);
            var orderSvc = new OrderServices(new Repositories.OrderRepository(_client));
            var resultOrder = await orderSvc.UpdateOrderStatus(updateOrderStatus);

            var orderBuyerSvc = new OrderBuyerServices(new Repositories.OrderBuyerRepository(_client));
            var resultBuyer = await orderBuyerSvc.UpdateOrderStatus(updateOrderStatus);

            var orderStoreSvc = new OrderStoreServices(new Repositories.OrderStoreRepository(_client));
            var resultStore = await orderSvc.UpdateOrderStatus(updateOrderStatus);

            log.LogWarning(" ");
            log.LogWarning("=== Order Status Updated ===");
            log.LogWarning($"Order ID      : {resultOrder.Id}");
            log.LogWarning($"Store ID      : {updateOrderStatus.storeId}");
            log.LogWarning($"Status        : {updateOrderStatus.status}");
            log.LogWarning(" ");
            return new OkObjectResult(resultOrder);
        }
    }
}
