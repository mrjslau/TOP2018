using System.Collections.Generic;
using System.IO;
using System.Linq;
using TensorFlow;

namespace ImageRecognition.Classificators
{
    public class TensorFlowClassificator : IImageClassifying
    {
        private static string TensorFlowLabelsFilePath { get; } =
            Path.Combine("resources", "model", "labels.txt");

        private static string TensorFlowModelFilePath { get; } =
            Path.Combine("resources", "model", "model.pb");
        
        private Dictionary<string, float> ClassifyTensor(TFTensor tensor, byte[] model, IReadOnlyList<string> labels)
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

        public Dictionary<string, float> ClassifyImage(byte[] image)
        {
            var tensor = TfImageUtil.CreateTensorFromBytes(image);
            
            // Use default settings for now
            var model = File.ReadAllBytes(TensorFlowModelFilePath);
            var labels = File.ReadAllLines(TensorFlowLabelsFilePath);

            return ClassifyTensor(tensor, model, labels);
        }
    }
}
