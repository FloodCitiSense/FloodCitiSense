using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Xamarin.Forms.GoogleMaps;

namespace IIASA.FloodCitiSense.Helper
{
    public class CameraPositionConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CameraPosition);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var pos = jo["Target"].ToObject<Position>();
            var zoom = (double)jo["Zoom"];
            var tilt = (double)jo["Tilt"];
            var bearing = (double)jo["Bearing"];

            return new CameraPosition(pos, zoom, tilt, bearing);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}