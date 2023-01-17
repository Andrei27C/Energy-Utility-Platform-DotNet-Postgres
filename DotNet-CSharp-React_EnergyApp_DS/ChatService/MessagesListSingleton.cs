namespace GrpcChat.Server;

public class MessagesListSingleton
{
    private static MessagesListSingleton instance = null;
    private static readonly object padlock = new object();
    private static List<ChatMessage> _messages;
    MessagesListSingleton()
    {
        _messages = new List<ChatMessage>();
    }

    public static MessagesListSingleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new MessagesListSingleton();
                }
                return instance;
            }
        }
    }
    public List<ChatMessage> List { get { return _messages; } }
    
    public void AddString(ChatMessage chatMessage) { _messages.Add(chatMessage); }


}