namespace Veggy.Core.Timer.Commands
{
    public class SquashPomodoro : Command
    {
        public string TimerId { get; }
        public string Reason { get; }

        public SquashPomodoro(string conversationId, string timerId, string reason)
            : base(conversationId)
        {
            TimerId = timerId;
            Reason = reason;
        }
    }
}