using System.Collections.Generic;
using System.IO;
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

        public static Dictionary<string, float> ClassifyImage(byte[] image)
        {
            var tensor = TfImageUtil.CreateTensorFromBytes(image);
            
            // Use default settings for now
            var model = File.ReadAllBytes(Program.TensorFlowModelFilePath);
            var labels = File.ReadAllLines(Program.TensorFlowLabelsFilePath);

            return ClassifyTensor(tensor, model, labels);
        }
    }
}