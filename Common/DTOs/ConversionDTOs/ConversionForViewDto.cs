namespace Common.DTOs.ConversionDTOs
{
    public class ConversionForViewDto
    {
        public string FromCurrency { get; set; } = null!;
        public string ToCurrency { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal Result { get; set; }
        public string Date { get; set; } = null!;
    }
}
