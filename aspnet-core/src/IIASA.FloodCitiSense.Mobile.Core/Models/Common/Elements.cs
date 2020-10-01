// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using IIASA.FloodCitiSense.Mobile.Core.Models.Common;
//
//    var elements = Elements.FromJson(jsonString);

namespace IIASA.FloodCitiSense.Mobile.Core.Models.Common
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class Elements
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("sections", NullValueHandling = NullValueHandling.Ignore)]
        public List<Section> Sections { get; set; }
    }

    public partial class Section
    {
        [JsonProperty("header", NullValueHandling = NullValueHandling.Ignore)]
        public string Header { get; set; }

        [JsonProperty("footer", NullValueHandling = NullValueHandling.Ignore)]
        public string Footer { get; set; }

        [JsonProperty("elements", NullValueHandling = NullValueHandling.Ignore)]
        public List<Element> Elements { get; set; }
    }

    public partial class Element
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore)]
        public string Caption { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public Value? Value { get; set; }

        [JsonProperty("on", NullValueHandling = NullValueHandling.Ignore)]
        public string On { get; set; }

        [JsonProperty("off", NullValueHandling = NullValueHandling.Ignore)]
        public string Off { get; set; }

        [JsonProperty("placeholder", NullValueHandling = NullValueHandling.Ignore)]
        public string Placeholder { get; set; }

        [JsonProperty("keyboard", NullValueHandling = NullValueHandling.Ignore)]
        public string Keyboard { get; set; }

        [JsonProperty("return-key", NullValueHandling = NullValueHandling.Ignore)]
        public string ReturnKey { get; set; }

        [JsonProperty("capitalization", NullValueHandling = NullValueHandling.Ignore)]
        public string Capitalization { get; set; }

        [JsonProperty("autocorrect", NullValueHandling = NullValueHandling.Ignore)]
        public string Autocorrect { get; set; }

        [JsonProperty("style", NullValueHandling = NullValueHandling.Ignore)]
        public string Style { get; set; }

        [JsonProperty("ontap", NullValueHandling = NullValueHandling.Ignore)]
        public string Ontap { get; set; }

        [JsonProperty("background", NullValueHandling = NullValueHandling.Ignore)]
        public string Background { get; set; }

        [JsonProperty("lines", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? Lines { get; set; }

        [JsonProperty("linebreak", NullValueHandling = NullValueHandling.Ignore)]
        public string Linebreak { get; set; }

        [JsonProperty("accessory", NullValueHandling = NullValueHandling.Ignore)]
        public string Accessory { get; set; }

        [JsonProperty("font", NullValueHandling = NullValueHandling.Ignore)]
        public string Font { get; set; }

        [JsonProperty("textcolor", NullValueHandling = NullValueHandling.Ignore)]
        public string Textcolor { get; set; }

        [JsonProperty("detailfont", NullValueHandling = NullValueHandling.Ignore)]
        public string Detailfont { get; set; }

        [JsonProperty("detailcolor", NullValueHandling = NullValueHandling.Ignore)]
        public string Detailcolor { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    public partial struct Value
    {
        public bool? Bool;
        public string String;

        public static implicit operator Value(bool Bool) => new Value { Bool = Bool };
        public static implicit operator Value(string String) => new Value { String = String };
    }

    public partial class Elements
    {
        public static Elements FromJson(string json) => JsonConvert.DeserializeObject<Elements>(json, IIASA.FloodCitiSense.Mobile.Core.Models.Common.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Elements self) => JsonConvert.SerializeObject(self, IIASA.FloodCitiSense.Mobile.Core.Models.Common.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                ValueConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class ValueConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Value) || t == typeof(Value?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Boolean:
                    var boolValue = serializer.Deserialize<bool>(reader);
                    return new Value { Bool = boolValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Value { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type Value");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Value)untypedValue;
            if (value.Bool != null)
            {
                serializer.Serialize(writer, value.Bool.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type Value");
        }

        public static readonly ValueConverter Singleton = new ValueConverter();
    }
}
