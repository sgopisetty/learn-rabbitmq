using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

const string queueName = "hello";

channel.QueueDeclare(queue: queueName, exclusive: false, autoDelete: false);
System.Console.WriteLine(" [*] Waiting for messages..");

var consumer = new EventingBasicConsumer(channel);
consumer.Received   +=  (model,ea) =>
{
    var body = ea.Body.ToArray(); 
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
};

channel.BasicConsume(queue:queueName, autoAck: true, consumer: consumer);
Console.ReadLine();