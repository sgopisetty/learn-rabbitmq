using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory{ HostName="localhost"};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

const string queueName = "hello";
channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete:false, arguments: null);

string message = $"Hello, World at {DateTime.Now.ToLongTimeString()}!";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: string.Empty, 
                    routingKey: queueName, 
                    basicProperties: null, 
                    body: body
);
