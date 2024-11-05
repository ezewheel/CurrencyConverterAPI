using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public int? SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
        public DateTime? SubscribedUntil { get; set; }
        public int ConversionsCount { get; set; }
        public bool usedTrial { get; set; }
        public bool isDeleted { get; set; }
    }
}