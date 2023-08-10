using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace UdemyRabbitMQ.subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://dziltqze:1VhtcPDfsWQNjwddF717AAsrWBfOSsQw@toad.rmq.cloudamqp.com/dziltqze");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            var randomQueueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(randomQueueName, "logs-fanout", "", null);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(randomQueueName,false, consumer);
            Console.WriteLine("Logları dinleniyor...");

            consumer.Received +=  (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine("Gelen Mesaj: " + message);

                channel.BasicAck(e.DeliveryTag, false);

                
            };


            Console.ReadLine();
        }

        
    }
}
