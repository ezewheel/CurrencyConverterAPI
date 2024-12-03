using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public decimal ConversionRate { get; set; }
        public string? CountryCode { get; set; }
        public bool IsDeleted { get; set; }

    }
}
