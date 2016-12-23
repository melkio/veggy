namespace Veggy.Core
{
    public abstract class Command
    {
        public string ConversationId { get;  }

        protected Command(string conversationId)
        {
            ConversationId = conversationId;
        }
    }
}