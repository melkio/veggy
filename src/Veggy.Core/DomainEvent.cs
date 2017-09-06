using System.Linq;

namespace Veggy.Core
{
    public abstract class DomainEvent 
    {
        public string ConversationId { get; }

        protected DomainEvent(string conversationId)
        {
            ConversationId = conversationId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var result = GetType()
                .GetProperties()
                .All(prop =>
                {
                    var value = prop.GetValue(this);
                    var targetValue = prop.GetValue(obj);

                    return value?.Equals(targetValue) ?? targetValue == null;
                });
            return result;
        }
    }
}