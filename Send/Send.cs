using RabbitMQ.Client;
using System.Text;

// connect to the server
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();


// declare a queue for us to send to (idempotent)
await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

const string message = "Hello World!";
var body = Encoding.UTF8.GetBytes(message);


// publish a message to the queue
await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

// When the code above finishes running, the channel and the connection will be disposed.
