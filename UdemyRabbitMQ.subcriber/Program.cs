﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
//factory.Uri = new Uri("amqps://kozfvhtn:eRpeWdWSmW91rQ-DFvwVYnVpwsNfEF9j@woodpecker.rmq.cloudamqp.com/kozfvhtn");
factory.HostName = "localhost";

//bağlantının kurulması
using var connection = factory.CreateConnection();

//kanalın oluşturulması
var channel = connection.CreateModel();


channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);
var queueName = "direct-queue-Critical";

channel.BasicConsume(queueName, false, consumer);
Console.WriteLine("Loglar dinleniyor.");
consumer.Received += (sender, args) =>
{
    var message = Encoding.UTF8.GetString(args.Body.ToArray());
    Console.WriteLine("Gelen mesaj : " + message);

    //File.AppendAllText("log-critical.txt", message+ "\n");

    Thread.Sleep(1000);
    channel.BasicAck(args.DeliveryTag, false);
};

Console.ReadLine();




