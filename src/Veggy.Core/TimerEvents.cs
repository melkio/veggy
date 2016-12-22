using System;

namespace Veggy.Core
{
    public partial class Timer
    {
        public class PomodoroStarted
        {
            public TimeSpan Duration { get; }

            public PomodoroStarted(TimeSpan duration)
            {
                Duration = duration;
            }
        }

        public class PomodoroSquashed
        {
            public string Reason { get; }

            public PomodoroSquashed(string reason)
            {
                Reason = reason;
            }
        }

        public class PomodoroCompleted
        {
        }
    }
}