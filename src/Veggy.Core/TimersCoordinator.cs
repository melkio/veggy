using Akka.Actor;

namespace Veggy.Core
{
    public class TimersCoordinator : ReceiveActor
    {
        private readonly IActorRef eventStore;

        public TimersCoordinator(IActorRef eventStore)
        {
            this.eventStore = eventStore;

            Receive<Timer.StartPomodoro>(command => HandleStartPomodoroCommand(command));
            Receive<Timer.PomodoroStarted>(domainEvent => HandlePomodoroStartedEvent(domainEvent));
        }

        private void HandleStartPomodoroCommand(Timer.StartPomodoro command)
        {
            var timer = Context.Child(command.TimerId);
            if (timer == ActorRefs.Nobody)
                timer = Context.ActorOf<Timer>(command.TimerId);

            timer.Tell(command);
        }

        private void HandlePomodoroStartedEvent(Timer.PomodoroStarted domainEvent)
        {
            eventStore.Tell(domainEvent);
        }
    }
}
