using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();

// canlı rabbitmq ya bağlanmak için
//factory.Uri = new Uri("amqps://kozfvhtn:eRpeWdWSmW91rQ-DFvwVYnVpwsNfEF9j@woodpecker.rmq.cloudamqp.com/kozfvhtn");

// dockerdaki rabbitmq ya bağlanmak için
factory.HostName = "localhost";
//bağlantının kurulması
using var connection = factory.CreateConnection();

//kanalın oluşturulması
var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
{
    var routeKey = $"route-{x}";
    var queueName = $"direct-queue-{x}";
    channel.QueueDeclare(queueName, true, false, false);
    channel.QueueBind(queueName, "logs-direct", routeKey,null);
});

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log = (LogNames)new Random().Next(1,5);

    string message = $"log-type: {log}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    var routeKey = $"route-{log}";

    //kuyruğa mesaj gönderme
    channel.BasicPublish("logs-direct", routeKey, null, messageBody);
    Console.WriteLine($"Log gönderilmiştir -> {message}");

});

Console.ReadLine();




public enum LogNames
{
    Critical = 1,
    Error = 2,
    Warnings = 3,
    Info = 4
}

