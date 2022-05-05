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

namespace API
{
    public static class OrderFunctions
    {
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
            return new OkObjectResult(result);
        }
    }
}
