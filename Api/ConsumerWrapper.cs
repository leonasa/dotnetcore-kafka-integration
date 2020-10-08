using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;

namespace Api
{
    public class ConsumerWrapper
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly string _topicName;

        public ConsumerWrapper(ConsumerConfig config, string topicName)
        {
            _topicName = topicName;
            _consumerConfig = config;
        }

        public IEnumerable<string> ReadMessage(CancellationToken cancellationToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
            consumer.Subscribe(_topicName);
            while (true)
            {
                var consumeResult = consumer.Consume(cancellationToken);
                yield return consumeResult.Message.Value;
            }
        }
    }
}