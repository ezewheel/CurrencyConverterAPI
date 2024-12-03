using Common.Enums;

namespace Data.Entities
{
    public class Subscription
    {
        public SubscriptionTypeEnum Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int? ConversionsLimit { get; set; }
    }
}
