using System;
using Akka.Actor;
using Akka.TestKit.NUnit3;
using NUnit.Framework;
using Veggy.Core.Timer;
using Veggy.Core.Timer.Commands;
using Veggy.Core.Timer.Events;

namespace Veggy.Core.Tests
{
    [TestFixture]
    public class TimersGatewayTest : TestKit
    {
        [Test]
        public void Start_pomodoro_command()
        {
            var probe = CreateTestProbe("probe");

            var command = new StartPomodoro("my-conversation-id", "my-timer-id", TimeSpan.FromSeconds(1));
            var coordinator = Sys.ActorOf(Props.Create(() => new TimersGateway(probe)));

            coordinator.Tell(command);

            var domainEvent = probe.ExpectMsg<PomodoroStarted>();
            Assert.AreEqual("my-conversation-id", domainEvent.ConversationId);
            Assert.AreEqual("my-timer-id", domainEvent.TimerId);
            Assert.AreEqual(TimeSpan.FromSeconds(1), domainEvent.Duration);
        }

        [Test]
        public void Start_pomodoro_command_for_two_timers()
        {
            var probe = CreateTestProbe("probe");

            var coordinator = Sys.ActorOf(Props.Create(() => new TimersGateway(probe)));

            coordinator.Tell(new StartPomodoro("my-conversation-id", "my-timer-id-1", TimeSpan.FromSeconds(1)));
            coordinator.Tell(new StartPomodoro("my-conversation-id", "my-timer-id-2", TimeSpan.FromSeconds(5)));

            probe.ExpectMsgAllOf(
                new PomodoroStarted("my-conversation-id", "my-timer-id-1", TimeSpan.FromSeconds(1)),
                new PomodoroStarted("my-conversation-id", "my-timer-id-2", TimeSpan.FromSeconds(5)));
        }
    }
}