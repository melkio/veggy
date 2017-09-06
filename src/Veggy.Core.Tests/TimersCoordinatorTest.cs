using System;
using Akka.Actor;
using Akka.TestKit.NUnit3;
using NUnit.Framework;

namespace Veggy.Core.Tests
{
    [TestFixture]
    public class TimersCoordinatorTest : TestKit
    {
        [Test]
        public void Start_pomodoro_command()
        {
            var probe = CreateTestProbe("probe");

            var command = new Timer.StartPomodoro("my-conversation-id", "my-timer-id", TimeSpan.FromSeconds(1));
            var coordinator = Sys.ActorOf(Props.Create(() => new TimersCoordinator(probe)));

            coordinator.Tell(command);

            var domainEvent = probe.ExpectMsg<Timer.PomodoroStarted>();
            Assert.AreEqual("my-conversation-id", domainEvent.ConversationId);
            Assert.AreEqual("my-timer-id", domainEvent.TimerId);
            Assert.AreEqual(TimeSpan.FromSeconds(1), domainEvent.Duration);
        }

        [Test]
        public void Start_pomodoro_command_for_two_timers()
        {
            var probe = CreateTestProbe("probe");

            var coordinator = Sys.ActorOf(Props.Create(() => new TimersCoordinator(probe)));

            coordinator.Tell(new Timer.StartPomodoro("my-conversation-id", "my-timer-id-1", TimeSpan.FromSeconds(1)));
            coordinator.Tell(new Timer.StartPomodoro("my-conversation-id", "my-timer-id-2", TimeSpan.FromSeconds(5)));

            probe.ExpectMsgAllOf(
                new Timer.PomodoroStarted("my-conversation-id", "my-timer-id-1", TimeSpan.FromSeconds(1)),
                new Timer.PomodoroStarted("my-conversation-id", "my-timer-id-2", TimeSpan.FromSeconds(5)));
        }
    }
}