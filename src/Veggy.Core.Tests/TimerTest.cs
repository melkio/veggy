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
            IgnoreMessages(message =>
            {
                var target = message as Timer.PomodoroStarted;
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


    }
}