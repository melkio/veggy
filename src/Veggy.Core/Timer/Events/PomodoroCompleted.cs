namespace Veggy.Core.Timer.Events
{
    public class PomodoroCompleted : DomainEvent
    {
        public string TimerId { get; }

        public PomodoroCompleted(string conversationId, string timerId) 
            : base(conversationId)
        {
            TimerId = timerId;
        }
    }
}