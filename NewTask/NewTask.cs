using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory{ HostName="localhost"};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

const string queueName = "hello";
channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete:false, arguments: null);

string message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

var properties = channel.CreateBasicProperties();
properties.Persistent = true;

channel.BasicPublish(exchange: string.Empty, 
                    routingKey: queueName, 
                    basicProperties: properties, 
                    body: body
);

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
    //return $"Hello, World at {DateTime.Now.ToLongTimeString()}!";    
}