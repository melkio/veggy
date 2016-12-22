using Akka.Actor;

namespace Veggy.Core
{
    public partial class Timer : ReceiveActor
    {
        private bool isTicking;

        public Timer()
        {
            Receive<StartPomodoro>(command => !isTicking, HandleStartPomodoro);
        }

        private void HandleStartPomodoro(StartPomodoro command)
        {
            isTicking = true;
            Sender.Tell(new PomodoroStarted(command.Duration));
        }
    }
}