using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API.DTO.Order;
using AutoMapper;
using BLL;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API
{
    public class PaymentFunctions
    {
        private readonly IMapper _mapper;
        private readonly CosmosClient _client;

        public PaymentFunctions(CosmosClient client)
        {
            _client = client;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderPayment, Payment>();
            });
            _mapper = config.CreateMapper();
        }

        [FunctionName("EventHubOrderEvents")]
        public async Task EventHubOrderEvents(
            [EventHubTrigger("f39-evh-tutorial-order", Connection = "eventHubConnectionString")] EventData[] events,
            ILogger log)
        {
            var exceptions = new List<Exception>();
            Payment payment = new Payment();
            foreach (EventData eventData in events)
            {
                try
                {
                    dynamic messages = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(eventData.Body.Array));
                    foreach (dynamic message in messages)
                    {
                        log.LogWarning(" ");
                        log.LogWarning("=== EventHub Order Events ===");
                        string subject = message.subject;
                        log.LogWarning(subject);
                        log.LogWarning(" ");
                        if (subject.StartsWith("Create"))
                        {
                            string content = message.data;
                            Order data = JsonConvert.DeserializeObject<Order>(content);
                            payment = _mapper.Map<Payment>(data.payment);
                            payment.orderId = data.Id;
                            var paymentSvc = new PaymentServices(new Repositories.PaymentRepository(_client));
                            var result = await paymentSvc.CreatePayment(payment);
                        } else if (subject.StartsWith("Update"))
                        {
                            string content = message.data;
                            Order data = JsonConvert.DeserializeObject<Order>(content);
                            if(data.payment.status == "paid")
                            {
                                log.LogWarning(" ");
                                log.LogWarning("Seller got order notification");
                                log.LogWarning(" ");
                            }
                        }
                    }
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }

        [FunctionName("CosmosDBPaymentUpdated")]
        public async Task CosmosDBPaymentUpdatedAsync([CosmosDBTrigger(
            databaseName: "Order",
            collectionName: "Payment",
            ConnectionStringSetting = "f39-cosmos-tutorial-string",
            LeaseCollectionName = "leases", CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            string message;
            if (input != null && input.Count > 0)
            {
                foreach (var inputItem in input)
                {
                    Payment payment = JsonConvert.DeserializeObject<Payment>(inputItem.ToString());
                    PaymentDetailStatus status = payment.detailStatus;
                    if (status.created != null)
                    {
                        if (status.paid != null)
                        {
                            message = "Payment Paid";

                        }
                        else if (status.canceled != null)
                        {
                            message = "Payment Canceled";
                        }
                        else
                        {
                            message = "Payment Created";
                        }

                    }
                    else
                    {
                        message = "Payment Error";
                    }

                    log.LogWarning(" ");
                    log.LogWarning("=== Payment Info ===");
                    log.LogWarning(message);
                    log.LogWarning($"Payment ID: {payment.Id}");
                    log.LogWarning($"Order ID  : {payment.orderId}");
                    log.LogWarning(" ");

                    var httpClient = HttpClientFactory.Create();
                    var url = "http://localhost:7071/api/order/payment";
                    UpdateOrderPaymentDTO updateOrderPayment = new UpdateOrderPaymentDTO
                    {
                        id = payment.Id,
                        detailStatus = payment.detailStatus,
                        status = payment.status,
                        method = payment.method,
                        orderId = payment.orderId,
                        total = payment.total,
                    };
                    HttpContent httpContent = new StringContent(
                    JsonConvert.SerializeObject(updateOrderPayment),
                    Encoding.UTF8,
                    "application/json");
                    dynamic result = await httpClient.PutAsync(url, httpContent);
                    await Task.Yield();
                }
            }
        }
    }
}
