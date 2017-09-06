namespace Veggy.Core.Timer.Commands
{
    public class CompletePomodoro : Command
        {
            public string TimerId { get; }

            public CompletePomodoro(string conversationId, string timerId)
                : base(conversationId)
            {
                TimerId = timerId;
            }
        }
}
