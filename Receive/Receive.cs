using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// connect to channel
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// declare queue 
// matches to the queue that 'Send' publishes to
// We declare queue here TOO because we might start the consumer before the publisher.
// (We want to make sure the queue exists before we try to consume messages from it)
await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");


// Since 'Send' will push 'Receive' messages asynchronously, we provide a callback.
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync("hello", autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();