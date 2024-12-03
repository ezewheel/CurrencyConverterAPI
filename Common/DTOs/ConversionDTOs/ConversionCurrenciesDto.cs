namespace Common.DTOs.ConversionDTOs
{
    public class ConversionCurrenciesDto
    {
        public required string From { get; set; }
        public required string To { get; set; }
        public decimal Amount { get; set; }
    }
}
