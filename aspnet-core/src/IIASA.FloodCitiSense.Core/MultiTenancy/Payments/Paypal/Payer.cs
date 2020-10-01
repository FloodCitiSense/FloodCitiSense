using Newtonsoft.Json;

namespace IIASA.FloodCitiSense.MultiTenancy.Payments.Paypal
{
    public class Payer
    {
        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }
    }
}