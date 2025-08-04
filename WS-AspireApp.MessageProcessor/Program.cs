namespace WS_AspireApp.MessageProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            
            builder.AddServiceDefaults();
            builder.Services.AddHostedService<Worker>();

            builder.AddRabbitMQClient("rabbitmqmessaging");

            var host = builder.Build();
            host.Run();
        }
    }
}