using System;
using Akka.Actor;
using Akka.TestKit.NUnit3;
using NUnit.Framework;

namespace Veggy.Core.Tests
{
    [TestFixture]
    public class TimerTest : TestKit
    {
        [Test]
        public void Start_pomodoro_command()
        {
            var duration = TimeSpan.FromSeconds(1);
            var command = new Timer.StartPomodoro("conversation-id", "timer-id", duration);
            var timer = Sys.ActorOf(Props.Create<Timer>());

            timer.Tell(command);

            var message = ExpectMsg<Timer.PomodoroStarted>();
            Assert.AreEqual(duration, message.Duration);
        }

        [Test]
        public void Start_pomodoro_command_fails_when_another_pomodoro_is_ticking()
        {
            var duration = TimeSpan.FromSeconds(10);

            IgnoreMessages(m =>
            {
                var target = m as Timer.PomodoroStarted;
                if (target == null)
                    return false;

                return target.Duration == duration;
            });

            var timer = Sys.ActorOf(Props.Create<Timer>());

            timer.Tell(new Timer.StartPomodoro("conversation-id", "timer-id", duration));
            timer.Tell(new Timer.StartPomodoro("conversation-id", "timer-id", TimeSpan.FromSeconds(5)));

            ExpectNoMsg();
        }

        [Test]
        public void Squash_pomodoro_command_when_pomodoro_is_ticking()
        {
            IgnoreMessages(m => m is Timer.PomodoroStarted);

            const string reason = "reason";
            var command = new Timer.SquashPomodoro("conversation-id", "timer-id", reason);
            var timer = Sys.ActorOf(Props.Create<Timer>());

            timer.Tell(new Timer.StartPomodoro("conversation-id", "timer-id", TimeSpan.FromSeconds(1)));
            timer.Tell(command);

            var message = ExpectMsg<Timer.PomodoroSquashed>();
            Assert.AreEqual(reason, message.Reason);
        }

        [Test]
        public void Squash_pomodoro_command_fails_when_pomodoro_is_not_ticking()
        {
            const string reason = "reason";
            var command = new Timer.SquashPomodoro("conversation-id", "timer-id", reason);
            var timer = Sys.ActorOf(Props.Create<Timer>());

            timer.Tell(command);

            ExpectNoMsg();
        }

        [Test]
        public void Should_complete_after_duration()
        {
            IgnoreMessages(m => m is Timer.PomodoroStarted);

            var duration = TimeSpan.FromSeconds(1);
            var command = new Timer.StartPomodoro("conversation-id", "timer-id", duration);
            var timer = Sys.ActorOf(Props.Create<Timer>());

            timer.Tell(command);

            ExpectMsg<Timer.PomodoroCompleted>();
        }

        [Test]
        public void Complete_pomodoro_command_fails_when_pomodoro_is_not_ticking()
        {
            IgnoreMessages(m => m is Timer.PomodoroStarted || m is Timer.PomodoroSquashed);

            var timer = Sys.ActorOf(Props.Create<Timer>());

            timer.Tell(new Timer.StartPomodoro("conversation-id", "timer-id", TimeSpan.FromSeconds(1)));
            timer.Tell(new Timer.SquashPomodoro("conversation-id", "timer-id", "reason"));

            ExpectNoMsg();
        }
    }
}