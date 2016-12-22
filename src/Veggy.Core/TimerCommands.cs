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
    }
}
