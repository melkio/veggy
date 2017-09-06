using Akka.Actor;
using Veggy.Core.Timer.Commands;
using Veggy.Core.Timer.Events;

namespace Veggy.Core.Timer
{
    public class TimerActor : ReceiveActor
    {
        private bool isTicking;

        public TimerActor()
        {
            Receive<StartPomodoro>(message => !isTicking, Handle);
            Receive<SquashPomodoro>(message => isTicking, Handle);
            Receive<CompletePomodoro>(message => isTicking, Handle);
        }

        private void Handle(StartPomodoro command)
        {
            isTicking = true;
            
            Context.System.Scheduler.ScheduleTellOnce(command.Duration, Self, new CompletePomodoro("conversation-id", "timer-id"), Sender);
            Sender.Tell(new PomodoroStarted(command.ConversationId, command.TimerId, command.Duration));
        }

        private void Handle(SquashPomodoro command)
        {
            isTicking = false;

            Sender.Tell(new PomodoroSquashed("conversation-id", "timer-id", command.Reason));
        }

        private void Handle(CompletePomodoro command)
        {
            isTicking = false;

            Sender.Tell(new PomodoroCompleted("conversation-id", "timer-id"));
        }
    }
}