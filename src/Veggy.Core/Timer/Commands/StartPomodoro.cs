using System;

namespace Veggy.Core.Timer.Commands
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
}