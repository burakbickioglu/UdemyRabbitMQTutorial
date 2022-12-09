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

channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

Random rnd = new Random();
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);

    var routeKey = $"{log1}.{log2}.{log3}";
    string message = $"log-type: {log1}-{log2}-{log3}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    //kuyruğa mesaj gönderme
    channel.BasicPublish("logs-topic", routeKey, null, messageBody);
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

