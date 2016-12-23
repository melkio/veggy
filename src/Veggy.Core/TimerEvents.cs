using System;

namespace Veggy.Core
{
    public partial class Timer
    {
        public class PomodoroStarted : DomainEvent
        {
            public string TimerId { get; }
            public TimeSpan Duration { get; }

            public PomodoroStarted(string conversationId, string timerId, TimeSpan duration)
                : base(conversationId)
            {
                TimerId = timerId;
                Duration = duration;
            }
        }

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
}