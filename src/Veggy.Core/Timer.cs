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
            Receive<CompletePomodoro>(command => HandleCompletePomodoro(command));
        }

        private void HandleStartPomodoro(StartPomodoro command)
        {
            isTicking = true;
            
            Context.System.Scheduler.ScheduleTellOnce(command.Duration, Self, new CompletePomodoro(), Sender);
            Sender.Tell(new PomodoroStarted(command.Duration));
        }

        private void HandleSquashPomodoro(SquashPomodoro command)
        {
            Sender.Tell(new PomodoroSquashed(command.Reason));
        }

        private void HandleCompletePomodoro(CompletePomodoro command)
        {
            Sender.Tell(new PomodoroCompleted());
        }
    }
}