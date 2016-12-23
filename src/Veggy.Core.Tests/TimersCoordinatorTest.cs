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
    }
}