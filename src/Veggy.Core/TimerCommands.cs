using System;

namespace Veggy.Core
{
    public partial class Timer
    {
        public class StartPomodoro
        {
            public TimeSpan Duration { get; }

            public StartPomodoro(TimeSpan duration)
            {
                Duration = duration;
            }
        }

        public class SquashPomodoro
        {
            public string Reason { get; }

            public SquashPomodoro(string reason)
            {
                Reason = reason;
            }
        }
    }
}
