using System;
using System.Threading.Tasks;

namespace WebSockets;

class Program
{
    static async Task Main(string[] args)
    {
        var client = new WebSocketClient("ws://localhost:8080");
        await client.ConnectAsync();
        await client.SendMessageAsync("Hello, Server!");
        var message = await client.ReceiveMessageAsync();
        Console.WriteLine("Received message: " + message);
        await client.CloseAsync();
    }
}
