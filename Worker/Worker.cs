using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

const string queueName = "hello";

channel.QueueDeclare(queue: queueName, exclusive: false, autoDelete: false, durable:true);
System.Console.WriteLine(" [*] Waiting for messages..");

var consumer = new EventingBasicConsumer(channel);
EventHandler<BasicDeliverEventArgs> receivedHandler = null;

receivedHandler   =  (model,ea) =>
{
    var body = ea.Body.ToArray(); 
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    if(message=="terminate"){
        throw new Exception("asdkfj");
    }
    int dots = message.Split('.').Length - 1;
    Thread.Sleep(dots * 10000);

    Console.WriteLine(" [x] Done");
    
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple:false);
    System.Console.WriteLine("Send ACK");

};

consumer.Received += receivedHandler;

channel.BasicConsume(queue:queueName, autoAck: false, consumer: consumer);
Console.ReadLine();