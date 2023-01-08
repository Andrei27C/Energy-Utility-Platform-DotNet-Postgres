using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebSockets;

public class WebSocketClient
{
    private readonly ClientWebSocket webSocket;
    private readonly Uri serverUri;

    public WebSocketClient(string serverUri)
    {
        this.webSocket = new ClientWebSocket();
        this.serverUri = new Uri(serverUri);
    }

    public async Task ConnectAsync()
    {
        await webSocket.ConnectAsync(serverUri, CancellationToken.None);
    }

    public async Task SendMessageAsync(string message)
    {
        var messageBuffer = System.Text.Encoding.UTF8.GetBytes(message);
        await webSocket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async Task<string> ReceiveMessageAsync()
    {
        var messageBuffer = new ArraySegment<byte>(new byte[4096]);
        var result = await webSocket.ReceiveAsync(messageBuffer, CancellationToken.None);
        if (result.MessageType == WebSocketMessageType.Close)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
        }
        return System.Text.Encoding.UTF8.GetString(messageBuffer.Array, messageBuffer.Offset, messageBuffer.Count);
    }

    public async Task CloseAsync()
    {
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
    }
}