using Flurl.Http.Configuration;
using ModernHttpClient;
using System.Net;
using System.Net.Http;

namespace IIASA.FloodCitiSense.ApiClient
{
    public class ModernHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new NativeMessageHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
        }
    }
}