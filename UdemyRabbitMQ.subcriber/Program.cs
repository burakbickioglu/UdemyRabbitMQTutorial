using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
//factory.Uri = new Uri("amqps://kozfvhtn:eRpeWdWSmW91rQ-DFvwVYnVpwsNfEF9j@woodpecker.rmq.cloudamqp.com/kozfvhtn");
factory.HostName = "localhost";

//bağlantının kurulması
using var connection = factory.CreateConnection();

//kanalın oluşturulması
var channel = connection.CreateModel();
channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);
var queueName = channel.QueueDeclare().QueueName;
Dictionary<string,object> headers = new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-match", "any");

channel.QueueBind(queueName, "header-exchange",string.Empty,headers);

channel.BasicConsume(queueName, false, consumer);
Console.WriteLine("Loglar dinleniyor.");
consumer.Received += (sender, args) =>
{
    var message = Encoding.UTF8.GetString(args.Body.ToArray());
    Console.WriteLine("Gelen mesaj : " + message);


    Thread.Sleep(500);
    channel.BasicAck(args.DeliveryTag, false);
};

Console.ReadLine();




