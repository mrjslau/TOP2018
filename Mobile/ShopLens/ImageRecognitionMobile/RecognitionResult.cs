using System.Collections.Generic;
using System.Linq;

namespace ImageRecognitionMobile
{
    public class RecognitionResult
    {
        public Dictionary<string, float> Predictions { get; set; } = new Dictionary<string, float>();
        public string WeightSpecifier { get; set; } = "";
        public string RawOcrResult { get; set; } = "";

        public string BestPrediction
        {
            get
            {
                return Predictions.OrderByDescending(x => x.Value)
                    .DefaultIfEmpty(new KeyValuePair<string, float>("", 0))
                    .First().Key;
            }
        }
    }
}