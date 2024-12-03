namespace Common.DTOs.CurrencyDTOs
{
    public class CurrencyForConversionViewDto
    {
        public required string Name { get; set; }
        public required string Symbol { get; set; }
        public string? CountryCode { get; set; }
    }
}
