using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ;

internal class Producer
{
    private const int DeviceId = 4;

    public static void Main()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare("logs", ExchangeType.Fanout);

            var csvValues = ParseCSV.readCSV();

            foreach (var value in csvValues)
            {
                //create sensor message
                // var asd = "{'hey': 1}";//GetMessage(args);
                var sensorMessage = new SensorMessage();
                sensorMessage.deviceId = DeviceId;
                sensorMessage.consumption = value;

                //create message to send
                var message = JsonConvert.SerializeObject(sensorMessage);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("logs",
                    "",
                    null,
                    body);

                Console.WriteLine(" [x] Sent {0}", message);

                Thread.Sleep(3000);
            }
        }


        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    private static string GetMessage(string[] args)
    {
        return args.Length > 0
            ? string.Join(" ", args)
            : "info: Hello World!";
    }
}