using System;
using System.Threading.Tasks;
using Api.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ProducerConfig _config;

        public OrderController(ProducerConfig config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] OrderRequest value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(value);

            Console.WriteLine("========");
            Console.WriteLine("Info: OrderController => Post => Recieved a new purchase order:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");

            var producer = new ProducerWrapper(_config, "orderrequests");
            await producer.WriteMessage(serializedOrder);

            return Created("TransactionId", "Your order is in progress");
        }
    }
}