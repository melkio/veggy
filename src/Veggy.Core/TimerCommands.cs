using System;

namespace Veggy.Core
{
    public partial class Timer
    {
        public class StartPomodoro : Command
        {
            public string TimerId { get; }
            public TimeSpan Duration { get; }

            public StartPomodoro(string conversationId, string timerId, TimeSpan duration)
                : base(conversationId)
            {
                TimerId = timerId;
                Duration = duration;
            }
        }

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
}
