using Common.Enums;

namespace Common.DTOs.ConversionDTOs
{
    public class ConversionResultDto
    {
        public ConversionResultEnum Status { get; set; }
        public decimal? Result { get; set; }
    }
}
