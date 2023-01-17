using Grpc.Core;
using GrpcChat;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace GrpcChat.Server
{
    public static class Connection
    {
        private static List<ChatMessage> _messages;
        public static ConcurrentDictionary<string, IServerStreamWriter<ChatMessage>> grpcChatConnections = new(); 
    }
}