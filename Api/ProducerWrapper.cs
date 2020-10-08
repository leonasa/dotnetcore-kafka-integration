using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Api
{
    public class ProducerWrapper
    {
        private readonly ProducerConfig _config;
        private readonly string _topicName;

        public ProducerWrapper(ProducerConfig config, string topicName)
        {
            _topicName = topicName;
            _config = config;
        }

        public async Task WriteMessage(string message)
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            var dr = producer.ProduceAsync(_topicName, new Message<Null, string> {Value = message});
            await dr.ContinueWith(task => { Console.WriteLine(task.IsFaulted ? "ERROR" : $"KAFKA => Delivered '{task.Result.Value}' to '{task.Result.Offset}'"); });
        }
    }
}