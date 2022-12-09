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

channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

Dictionary<string, object> headers = new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape2", "a4");

var properties = channel.CreateBasicProperties();
properties.Headers = headers;

channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes("header mesajım"));
Console.WriteLine("Mesaj gönderilmiştir.");
Console.ReadLine();


