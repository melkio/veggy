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

            Receive<StartPomodoro>(message => Handle(message));
            Receive<PomodoroStarted>(message => Handle(message));
        }

        private void Handle(StartPomodoro message)
        {
            var timer = Context.Child(message.TimerId);
            if (timer.IsNobody())
                timer = Context.ActorOf<TimerActor>(message.TimerId);

            timer.Tell(message);
        }

        private void Handle(PomodoroStarted message)
        {
            eventStore.Tell(message);
        }
    }
}
