// using System;
// using System.Net;
// using System.Net.WebSockets;
// using System.Threading;
// using System.Threading.Tasks;
//
// namespace WebSockets;
//
// public class WebSocketServer
// {
//     private readonly HttpListener listener;
//
//     public WebSocketServer(string serverUri)
//     {
//         listener = new HttpListener();
//         listener.Prefixes.Add(serverUri);
//     }
//
//     public async Task StartAsync()
//     {
//         listener.Start();
//         while (true)
//         {
//             var context = await listener.GetContextAsync();
//             if (context.Request.IsWebSocketRequest)
//             {
//                 ProcessWebSocketRequest(context);
//             }
//             else
//             {
//                 context.Response.StatusCode = 400;
//                 context.Response.Close();
//             }
//         }
//     }
//     
//     private async void ProcessWebSocketRequest(HttpListenerContext context)
//     {
//         WebSocket webSocket = null;
//         try
//         {
//             webSocket = await context.AcceptWebSocketAsync(subProtocol: null);
//             var buffer = new ArraySegment<byte>(new byte[4096]);
//             while (webSocket.State == WebSocketState.Open)
//             {
//                 var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
//                 if (result.MessageType == WebSocketMessageType.Close)
//                 {
//                     await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
//                 }
//                 else
//                 {
//                     // Process the received message
//                     var message = System.Text.Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
//                     Console.WriteLine("Received message: " + message);
//
//                     // Send a response message
//                     var responseMessage = "Hello, Client!";
//                     var responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseMessage);
//                     await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
//                 }
//             }
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine("Error: " + ex);
//         }
//         finally
//         {
//             if (webSocket != null)
//                 webSocket.Dispose();
//         }
//     }
//
// }
