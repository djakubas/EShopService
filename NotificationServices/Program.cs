using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Kafka;
using NotificationService.Application.Services;

namespace NotificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IOrderNotificationService, OrderNotificationService>();
                    services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
                })
                .Build();

            host.Run();
        }
    }
}