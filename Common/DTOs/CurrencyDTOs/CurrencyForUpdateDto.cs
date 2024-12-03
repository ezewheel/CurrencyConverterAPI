namespace Common.DTOs.CurrencyDTOs
{
    public class CurrencyForUpdateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Symbol { get; set; }
        public decimal ConversionRate { get; set; }
        public string? CountryCode { get; set; }
    }
}
