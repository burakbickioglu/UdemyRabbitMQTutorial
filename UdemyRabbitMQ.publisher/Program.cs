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

channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

Enumerable.Range(1, 50).ToList().ForEach(x => {
    string message = $"log {x}"; 
    var messageBody = Encoding.UTF8.GetBytes(message);

    //kuyruğa mesaj gönderme
    channel.BasicPublish("logs-fanout", "", null, messageBody);
    Console.WriteLine($"Mesaj gönderilmiştir -> {message}");

});

Console.ReadLine();






