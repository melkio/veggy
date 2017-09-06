using System;

namespace Veggy.Core.Timer.Events
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
}