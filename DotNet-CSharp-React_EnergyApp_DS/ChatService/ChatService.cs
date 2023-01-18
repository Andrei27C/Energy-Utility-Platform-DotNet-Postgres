using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcChat.Server
{
    public class ChatService : GrpcChat.ChatService.ChatServiceBase
    {
        private readonly ILogger<ChatService> _logger;
        private readonly Dictionary<string, IServerStreamWriter<ChatMessage>> _subscriptions = new Dictionary<string, IServerStreamWriter<ChatMessage>>();

        private static MessagesListSingleton _messages = MessagesListSingleton.Instance;
        
        public ChatService(ILogger<ChatService> logger)
        {
            _logger = logger;
        }

        public override async Task<Empty> SendMessage(ChatMessage request, ServerCallContext context)
        {
            var sender = request.Sender;
            var receiver = request.Receiver;
            var message = request.Message;

            _logger.LogInformation($"Received message '{message}' from sender '{sender}' for receiver '{receiver}'");

            if (_subscriptions.ContainsKey(receiver))
            {
                await _subscriptions[receiver].WriteAsync(request);
            }

            ChatMessage msg = new ChatMessage();
            msg.Sender = request.Sender;
            msg.Receiver = request.Receiver;
            msg.Message = request.Message;
            _messages.List.Add(msg);
            foreach (var chatMessage in _messages.List)
            {
                Console.WriteLine("123123123-------" + chatMessage);
            }
            return new Empty();
        }
        
        public override async Task ReceiveMessage(
            Client client,
            IServerStreamWriter<ChatMessage> responseStream,
            ServerCallContext context)
        {
            Console.WriteLine("Stream opened by " + client.Username);
            _subscriptions.Add(client.Username, responseStream);

            // while (await responseStream.WriteAsync(_messages.List)) {  }
            foreach (var message in _messages.List)
            {
                await responseStream.WriteAsync(message);
            }
            while (true)
            {
                
            }
            
        }
    }
}