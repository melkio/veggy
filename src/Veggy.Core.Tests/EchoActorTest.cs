using Akka.Actor;
using Akka.TestKit.NUnit3;
using NUnit.Framework;

namespace Veggy.Core.Tests
{
    [TestFixture]
    public class EchoActorTest : TestKit
    {
        [Test]
        public void Should_echo_the_request()
        {
            const string text = "Hello world";

            var request = new EchoActor.Request(text);
            var actor = Sys.ActorOf(Props.Create<EchoActor>());

            actor.Tell(request);

            var response = ExpectMsg<EchoActor.Response>();
            Assert.AreEqual(text, response.Text);
        }
    }
}