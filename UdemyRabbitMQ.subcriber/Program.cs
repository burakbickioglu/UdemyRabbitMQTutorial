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

//random kuyruk adı alındı
var randomQueueName = channel.QueueDeclare().QueueName;
//durable : true ise fiziksel olarak sabit diskte kaydedilir, false ise ram de tutulur.

// bir kuyruk exchange ye bind edildi, uygulama down olduğunda kuyruk ta down olacak
channel.QueueBind(randomQueueName, "logs-fanout", "", null);

// bir subcriber tek seferde kaç mesaj alacak bunun belirlenmesi
//channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);
channel.BasicQos(0, 1, false);

//kuyruğun oluşturulması, eğer kuyruk zaten var ise sorun olmaz.
//channel.QueueDeclare("hello-queue", true, false, false);


var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(randomQueueName, false, consumer);
Console.WriteLine("Loglar dinleniyor.");
consumer.Received += (sender, args) =>
{
    var message = Encoding.UTF8.GetString(args.Body.ToArray());
    Console.WriteLine("Gelen mesaj : " + message);
    Thread.Sleep(1000);
    channel.BasicAck(args.DeliveryTag, false);
};

Console.ReadLine();




