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
            var command = new Timer.StartPomodoro(duration);
            var timer = Sys.ActorOf(Props.Create<Timer>());

            timer.Tell(command);

            var message = ExpectMsg<Timer.PomodoroStarted>();
            Assert.AreEqual(duration, message.Duration);
        }

        [Test]
        public void Start_pomodoro_command_fails_when_another_pomodoro_is_ticking()
        {
            IgnoreMessages(m =>
            {
                var target = m as Timer.PomodoroStarted;
                if (target == null)
                    return false;

                return target.Duration == TimeSpan.FromSeconds(1);
            });

            var duration = TimeSpan.FromSeconds(1);
            var command = new Timer.StartPomodoro(duration);
            var timer = Sys.ActorOf(Props.Create<Timer>());

            timer.Tell(command);
            timer.Tell(new Timer.StartPomodoro(TimeSpan.FromSeconds(5)));

            ExpectNoMsg();
        }

        [Test]
        public void Squash_pomodoro_command_when_pomodoro_is_ticking()
        {
            IgnoreMessages(m => m is Timer.PomodoroStarted);

            const string reason = "reason";
            var command = new Timer.SquashPomodoro(reason);
            var timer = Sys.ActorOf(Props.Create<Timer>());

            timer.Tell(new Timer.StartPomodoro(TimeSpan.FromSeconds(1)));
            timer.Tell(command);

            var message = ExpectMsg<Timer.PomodoroSquashed>();
            Assert.AreEqual(reason, message.Reason);
        }

        [Test]
        public void Squash_pomodoro_command_fails_when_pomodoro_is_not_ticking()
        {
            const string reason = "reason";
            var command = new Timer.SquashPomodoro(reason);
            var timer = Sys.ActorOf(Props.Create<Timer>());

            timer.Tell(command);

            ExpectNoMsg();
        }
    }
}