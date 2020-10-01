using Abp.Extensions;
using System;

namespace IIASA.FloodCitiSense.ApiClient
{
    public static class ApiUrlConfig
    {
        //TODO: Replace with PROD url.https://fcs.geo-wiki.org/
        //private const string DefaultHostUrl = "http://147.125.53.74:45455/";
        // private const string DefaultHostUrl = "http://192.168.94.193:45455/";
        private const string DefaultHostUrl = "https://fcs.geo-wiki.org/";

        public static string BaseUrl { get; private set; }

        static ApiUrlConfig()
        {
            ResetBaseUrl();
        }

        public static void ChangeBaseUrl(string baseUrl)
        {
            BaseUrl = ReplaceLocalhost(NormalizeUrl(baseUrl));
        }

        public static void ResetBaseUrl()
        {
            BaseUrl = ReplaceLocalhost(DefaultHostUrl);
        }

        public static bool IsLocal => DefaultHostUrl.Contains("localhost");

        private static string NormalizeUrl(string baseUrl)
        {
            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var uriResult) ||
                (uriResult.Scheme != "http" && uriResult.Scheme != "https"))
            {
                throw new ArgumentException("Unexpected base URL: " + baseUrl);
            }

            return uriResult.ToString().EnsureEndsWith('/');
        }

        private static string ReplaceLocalhost(string url)
        {
            return url.Replace("localhost", DebugServerIpAddresses.Current);
        }
    }
}