using System;
using Akka.Actor;
using Akka.TestKit.NUnit3;
using NUnit.Framework;
using Veggy.Core.Timer.Commands;
using Veggy.Core.Timer.Events;

namespace Veggy.Core.Tests
{
    [TestFixture]
    public class TimerActorTest : TestKit
    {
        [Test]
        public void Start_pomodoro_command()
        {
            var duration = TimeSpan.FromSeconds(1);
            var command = new StartPomodoro("conversation-id", "timer-id", duration);
            var timer = Sys.ActorOf(Props.Create<Timer.TimerActor>());

            timer.Tell(command);

            var message = ExpectMsg<PomodoroStarted>();
            Assert.AreEqual(duration, message.Duration);
        }

        [Test]
        public void Start_pomodoro_command_fails_when_another_pomodoro_is_ticking()
        {
            var duration = TimeSpan.FromSeconds(10);

            IgnoreMessages(m =>
            {
                var target = m as PomodoroStarted;
                if (target == null)
                    return false;

                return target.Duration == duration;
            });

            var timer = Sys.ActorOf(Props.Create<Timer.TimerActor>());

            timer.Tell(new StartPomodoro("conversation-id", "timer-id", duration));
            timer.Tell(new StartPomodoro("conversation-id", "timer-id", TimeSpan.FromSeconds(5)));

            ExpectNoMsg();
        }

        [Test]
        public void Squash_pomodoro_command_when_pomodoro_is_ticking()
        {
            IgnoreMessages(m => m is PomodoroStarted);

            const string reason = "reason";
            var command = new SquashPomodoro("conversation-id", "timer-id", reason);
            var timer = Sys.ActorOf(Props.Create<Timer.TimerActor>());

            timer.Tell(new StartPomodoro("conversation-id", "timer-id", TimeSpan.FromSeconds(1)));
            timer.Tell(command);

            var message = ExpectMsg<PomodoroSquashed>();
            Assert.AreEqual(reason, message.Reason);
        }

        [Test]
        public void Squash_pomodoro_command_fails_when_pomodoro_is_not_ticking()
        {
            const string reason = "reason";
            var command = new SquashPomodoro("conversation-id", "timer-id", reason);
            var timer = Sys.ActorOf(Props.Create<Timer.TimerActor>());

            timer.Tell(command);

            ExpectNoMsg();
        }

        [Test]
        public void Should_complete_after_duration()
        {
            IgnoreMessages(m => m is PomodoroStarted);

            var duration = TimeSpan.FromSeconds(1);
            var command = new StartPomodoro("conversation-id", "timer-id", duration);
            var timer = Sys.ActorOf(Props.Create<Timer.TimerActor>());

            timer.Tell(command);

            ExpectMsg<PomodoroCompleted>();
        }

        [Test]
        public void Complete_pomodoro_command_fails_when_pomodoro_is_not_ticking()
        {
            IgnoreMessages(m => m is PomodoroStarted || m is PomodoroSquashed);

            var timer = Sys.ActorOf(Props.Create<Timer.TimerActor>());

            timer.Tell(new StartPomodoro("conversation-id", "timer-id", TimeSpan.FromSeconds(1)));
            timer.Tell(new SquashPomodoro("conversation-id", "timer-id", "reason"));

            ExpectNoMsg();
        }
    }
}