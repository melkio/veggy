using Akka.Actor;

namespace Veggy.Core
{
    public class EchoActor : ReceiveActor
    {
        public EchoActor()
        {
            Receive<Request>(request => Sender.Tell(new Response(request.Text)));
        }

        public class Request
        {
            public string Text { get; }

            public Request(string text)
            {
                Text = text;
            }
        }

        public class Response
        {
            public string Text { get; }

            public Response(string text)
            {
                Text = text;
            }
        }
    }
}
