using System.Collections.Generic;
using System.Linq;
using TensorFlow;

namespace ImageRecognition
{
    public static class Classificator
    {
        public static Dictionary<string, float> ClassifyTensor(TFTensor tensor, byte[] model, IReadOnlyList<string> labels)
        {
            var graph = new TFGraph();
            graph.Import(model);

            Dictionary<string, float> classificationResults;
            using (var session = new TFSession(graph))
            {
                var runner = session.GetRunner();
                runner.AddInput(graph["Placeholder"][0], tensor).Fetch(graph["loss"][0]);
                var output = runner.Run();
                var result = output[0];

                // Results
                var probabilities = ((float[][]) result.GetValue(jagged: true))[0];

                classificationResults =
                    probabilities
                        .Select((x, i) => new KeyValuePair<string, float>(labels[i], probabilities[i]))
                        .ToDictionary(x => x.Key, x => x.Value);
            }

            return classificationResults;
        }
    }
}