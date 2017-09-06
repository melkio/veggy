using Akka.Actor;
using Veggy.Core.Timer.Commands;
using Veggy.Core.Timer.Events;

namespace Veggy.Core.Timer
{
    public class TimersGateway : ReceiveActor
    {
        private readonly IActorRef eventStore;

        public TimersGateway(IActorRef eventStore)
        {
            this.eventStore = eventStore;

            Receive<StartPomodoro>(command => HandleStartPomodoroCommand(command));
            Receive<PomodoroStarted>(domainEvent => HandlePomodoroStartedEvent(domainEvent));
        }

        private void HandleStartPomodoroCommand(StartPomodoro command)
        {
            var timer = Context.Child(command.TimerId);
            if (timer.IsNobody())
                timer = Context.ActorOf<TimerActor>(command.TimerId);

            timer.Tell(command);
        }

        private void HandlePomodoroStartedEvent(PomodoroStarted domainEvent)
        {
            eventStore.Tell(domainEvent);
        }
    }
}
