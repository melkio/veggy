namespace Veggy.Core.Timer.Events
{
    public class PomodoroSquashed : DomainEvent
    {
        public string TimerId { get; }
        public string Reason { get; }

        public PomodoroSquashed(string conversationId, string timerId, string reason)
            : base(conversationId)
        {
            TimerId = timerId;
            Reason = reason;
        }
    }
}