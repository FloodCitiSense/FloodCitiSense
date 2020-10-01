using IIASA.FloodCitiSense.Editions.Dto;

namespace IIASA.FloodCitiSense.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }
    }
}
