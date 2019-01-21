using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ImageRecognition.Classificators.Helpers
{
// Generated using https://app.quicktype.io/#l=cs&r=json2csharp
    public partial class CustomVisionPredictionResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("project")]
        public Guid Project { get; set; }

        [JsonProperty("iteration")]
        public Guid Iteration { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("predictions")]
        public Prediction[] Predictions { get; set; }
    }

    public class Prediction
    {
        [JsonProperty("probability")]
        public double Probability { get; set; }

        [JsonProperty("tagId")]
        public Guid TagId { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; }
    }

    public partial class CustomVisionPredictionResponse
    {
        public static CustomVisionPredictionResponse FromJson(string json) => JsonConvert.DeserializeObject<CustomVisionPredictionResponse>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this CustomVisionPredictionResponse self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}