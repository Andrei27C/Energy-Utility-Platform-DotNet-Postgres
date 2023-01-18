using System.Net.Http.Headers;
using System.Text;
using DotNet_CSharp_React_EnergyApp_DS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Npgsql;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ;

internal class Consumer
{
    private static int _numberOfMessagesReceived;
    const string requestUrl = "http://localhost:5024/api/ConsumptionPerDevice";
    private static double _lastConsumption;
    
    public static void Main()
    {
        //db connection
        DeviceManagerContext context = GetDatabaseContext();

        var factory = new ConnectionFactory { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare("logs", ExchangeType.Fanout);
            // declare a server-named queue
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "logs", "");

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);

                //create json object from message
                // var postMessage = new JObject();
                var jsonMessage = JObject.Parse(message);
                double receivedConsumption = Convert.ToDouble(jsonMessage.GetValue("consumption"));
                int receivedId = Convert.ToInt32(jsonMessage.GetValue("deviceId"));
                
                //create the database object to add
                var consumptionPerDevice = new ConsumptionPerDevice();
                consumptionPerDevice.DateTime = DateTime.UtcNow;
                consumptionPerDevice.ConsumptionPerHour = receivedConsumption;
                consumptionPerDevice.ConsumptionPerDeviceId = receivedId;

                
                
                _numberOfMessagesReceived++;
                if (_numberOfMessagesReceived % 6 == 0)
                {
                    Console.WriteLine("hereeeee");
                    //primary key update
                    consumptionPerDevice.ConsumptionPerDeviceId =  context.ConsumptionPerDevice.OrderBy(c => c.ConsumptionPerDeviceId).Last().ConsumptionPerDeviceId + 1;
                    
                    //calculate hourly consumption
                    consumptionPerDevice.ConsumptionPerHour -= _lastConsumption;
                   
                    //get devicemapping id
                    List<DeviceMapping> deviceMappings = context.DeviceMappings.ToList();
                    // Console.WriteLine("devices-------------------------");
                    var deviceMappingId = 1;
                    foreach (var deviceMapping in deviceMappings)
                    {
                        // Console.WriteLine(deviceMapping.DeviceMappingId + " - " + deviceMapping.DeviceId);
                        if (deviceMapping.DeviceId == consumptionPerDevice.ConsumptionPerDeviceId)
                            deviceMappingId = deviceMapping.DeviceMappingId;
                    }
                    
                    consumptionPerDevice.DeviceMappingId = 7;
                    
                    
                    //get Device id by device mapping id
                    List<DeviceMapping> dbDeviceMappings = context.DeviceMappings.ToList();
                    var deviceId = 1;
                    foreach (var deviceMapping in dbDeviceMappings)
                    {
                        // Console.WriteLine(deviceMapping.DeviceMappingId + " - " + consumptionPerDevice.DeviceMappingId);
                        if (deviceMapping.DeviceMappingId == consumptionPerDevice.DeviceMappingId)
                            deviceId = deviceMapping.DeviceId;
                    }
                    //get Device max consumption
                    List<Device> dbDevices = context.Devices.ToList();
                    var maxHourlyConsumption = 1;
                    foreach (var device in dbDevices)
                    {
                        // Console.WriteLine(device.DeviceId + " - " + consumptionPerDevice.DeviceMappingId);
                        if (device.DeviceId == deviceId)
                            maxHourlyConsumption = device.Consumption;
                    }

                    //verify max hourly consumption
                    if (consumptionPerDevice.ConsumptionPerHour > maxHourlyConsumption)
                    {
                        Console.WriteLine("----------------- Exceeded max hourly consumption: " + maxHourlyConsumption + ". Current consumption: " + consumptionPerDevice.ConsumptionPerHour);
                    }
                    
                    
                    //add to db
                    Task.Run(async () =>
                    {
                        context.ConsumptionPerDevice.Add(consumptionPerDevice);
                        await context.SaveChangesAsync();
                        context.ConsumptionPerDevice.AsNoTracking();
                    }).GetAwaiter().GetResult();
                    Console.WriteLine("Added to database");
                    _lastConsumption = receivedConsumption;
                }
            };
            channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine(" Press [enter] to exit.");
            // Console.ReadLine()
            while (true)
            {
                
            }
        }
    }

    private static DeviceManagerContext GetDatabaseContext()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("PostgresDb");
        var options = new DbContextOptionsBuilder<DeviceManagerContext>()
            .UseNpgsql(new NpgsqlConnection(connectionString))
            .Options;
        var context = new DeviceManagerContext(options);
        
        return context;
    }
    //
    // static async Task<Uri> CreateProductAsync(Device product)
    // {
    //     HttpResponseMessage response = await client.PostAsJsonAsync(
    //         "api/Device", product);
    //     response.EnsureSuccessStatusCode();
    //
    //     // return URI of the created resource.
    //     return response.Headers.Location;
    // }
    
    
    // postMessage = consumptionPerDevice.ToJson();
    //     
    // postMessage.Add("deviceMappingId",1);
    // postMessage.Add("dateTime",DateTime.Now);
    // postMessage.Add("consumptionPerHour", jsonMessage.GetValue("consumption"));
    // postMessage.AddHea
    // Console.WriteLine(" [p] {0}", postMessage);
}