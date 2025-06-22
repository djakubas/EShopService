using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Kafka;
using NotificationService.Application.Services;
using NotificationService.Domain.Models;
using static Confluent.Kafka.ConfigPropertyNames;

namespace NotificationService
{
    public class Worker : BackgroundService
    {
        private readonly IKafkaConsumer _kafkaConsumer;
        public Worker(IKafkaConsumer kafkaConsumer)
        {
            _kafkaConsumer = kafkaConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _kafkaConsumer.StartConsumingOrderEventsAsync(stoppingToken);
        }
    }
}
