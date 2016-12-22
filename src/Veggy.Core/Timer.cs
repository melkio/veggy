using Akka.Actor;

namespace Veggy.Core
{
    public partial class Timer : ReceiveActor
    {
        public Timer()
        {
            Receive<StartPomodoro>(command => Sender.Tell(new PomodoroStarted(command.Duration)));
        }
    }
}