using Akka.Actor;

namespace Veggy.Core
{
    public partial class Timer : ReceiveActor
    {
        private bool isTicking;

        public Timer()
        {
            Receive<StartPomodoro>(command => !isTicking, HandleStartPomodoro);
            Receive<SquashPomodoro>(command => isTicking, HandleSquashPomodoro);
        }

        private void HandleStartPomodoro(StartPomodoro command)
        {
            isTicking = true;
            Sender.Tell(new PomodoroStarted(command.Duration));
        }

        private void HandleSquashPomodoro(SquashPomodoro command)
        {
            Sender.Tell(new PomodoroSquashed(command.Reason));
        }
    }
}