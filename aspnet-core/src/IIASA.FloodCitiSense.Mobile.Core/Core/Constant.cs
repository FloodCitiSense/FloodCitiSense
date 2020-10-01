namespace IIASA.FloodCitiSense.Mobile.Core.Core
{
    public static class Constant
    {
        public const string WeatherApiKey = "xxxxxxxxx";

        public const string GeoWikiApiKey = "xxxxxxxxxxxxxxxx";

        public const string ImagesUploadUri = "https://geo-wiki.org/Application/code/uploadFloodCitiSensePhoto.php";

        public static string WeatherApiUrl =
            $"https://api.openweathermap.org/data/2.5/weather?&units=metric&lat={{0}}&lon={{1}}&appid={WeatherApiKey}&lang={{2}}";

        public const string DeviceApiHost = "https://tech.fondus.com.tw:14481/istsos/ensuf";

        public const string DeviceGeoJsonApi = "https://tech.fondus.com.tw:14481/istsos/wa/istsos/services/ensuf/procedures/operations/geojson";

        public const string ElementJson = "IIASA.FloodCitiSense.Mobile.Core.Data.element.json";

        public const string OfflineDatabaseName = "FloodCitiSense.db";
    }
}
