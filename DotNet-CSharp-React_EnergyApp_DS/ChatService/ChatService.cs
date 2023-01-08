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

            return new Empty();
        }

        public override async Task ReceiveMessage(
            Client request, 
            IServerStreamWriter<ChatMessage> responseStream, 
            ServerCallContext context)
        {
            var username = request.Username;

            _logger.LogInformation($"Received subscribe request from client '{username}'");

            _subscriptions.Add(username, responseStream);

            await Task.CompletedTask;
        }
    }
}