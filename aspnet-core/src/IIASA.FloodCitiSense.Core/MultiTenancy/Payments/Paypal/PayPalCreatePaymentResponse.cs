using Newtonsoft.Json;

namespace IIASA.FloodCitiSense.MultiTenancy.Payments.Paypal
{
    public class PayPalCreatePaymentResponse : CreatePaymentResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        public override string GetId()
        {
            return Id;
        }
    }
}