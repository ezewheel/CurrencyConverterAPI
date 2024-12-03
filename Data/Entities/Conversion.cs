using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Conversion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FromCurrencyId { get; set; }
        public Currency FromCurrency { get; set; } = null!;
        public int ToCurrencyId { get; set; }
        public Currency ToCurrency { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal Result { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
