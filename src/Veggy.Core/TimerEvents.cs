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
    }
}