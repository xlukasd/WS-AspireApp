using RabbitMQ.Client;
using System.Text;

namespace WS_AspireApp.DummyService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();

        builder.AddRabbitMQClient("rabbitmqmessaging");

        var app = builder.Build();

        app.MapDefaultEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapGet("/dummy", (IConnection connection) =>
        {
            var messageToPublish = "very good message sent at " + DateTime.UtcNow;
            var bytesToPublish = Encoding.UTF8.GetBytes(messageToPublish);

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("basic", "direct");

            channel.BasicPublish("basic", "basicevent", null, bytesToPublish);

            return Results.Ok($"Sent to RabbitMQ: {messageToPublish}");
        })
        .WithName("Dummy");        

        app.Run();
    }
}
