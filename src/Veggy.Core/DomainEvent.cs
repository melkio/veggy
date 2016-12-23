namespace Veggy.Core
{
    public abstract class DomainEvent
    {
        public string ConversationId { get; }

        protected DomainEvent(string conversationId)
        {
            ConversationId = conversationId;
        }
    }
}