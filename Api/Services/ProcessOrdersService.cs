using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Api.Services
{
    public class ProcessOrdersService : BackgroundService
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;
        private readonly CancellationToken _cancellationToken;

        public ProcessOrdersService(ConsumerConfig consumerConfig, ProducerConfig producerConfig, CancellationToken cancellationToken)
        {
            _producerConfig = producerConfig;
            _consumerConfig = consumerConfig;
            _cancellationToken = cancellationToken;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("OrderProcessing Service Started");

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumerHelper = new ConsumerWrapper(_consumerConfig, "orderrequests");
                var orderRequests = consumerHelper.ReadMessage(_cancellationToken);
                
                foreach (var orderRequest in orderRequests)
                {
                    //Deserilaize 
                    OrderRequest order = JsonConvert.DeserializeObject<OrderRequest>(orderRequest);
                    //TODO:: Process Order
                    Console.WriteLine($"Info: OrderHandler => Processing the order for {order.Productname}");
                    order.Status = OrderStatus.Completed;

                    var producerWrapper = new ProducerWrapper(_producerConfig, "readytoship");
                    await producerWrapper.WriteMessage(JsonConvert.SerializeObject(order));
                }
                //Write to ReadyToShip Queue
            }
        }
    }
}