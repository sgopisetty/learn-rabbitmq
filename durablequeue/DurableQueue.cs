using RabbitMQ.Client;
using System;
using System.Text;

class Program
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Declare a durable queue
            string queueName = "durable_queue";
            bool durable = true;
            bool exclusive = false;
            bool autoDelete = false;
            channel.QueueDeclare(queue: queueName,
                                 durable: durable,
                                 exclusive: exclusive,
                                 autoDelete: autoDelete,
                                 arguments: null);

            string message = "Hello, this is a persistent message!";
            var body = Encoding.UTF8.GetBytes(message);

            // Create persistent message properties
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            // Publish the message
            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine($" [x] Sent persistent message: {message}");
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}