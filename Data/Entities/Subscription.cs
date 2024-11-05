using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int? ConversionsLimit { get; set; }
        public ICollection<User> Subscribers { get; set; } = new List<User>();
    }
}
