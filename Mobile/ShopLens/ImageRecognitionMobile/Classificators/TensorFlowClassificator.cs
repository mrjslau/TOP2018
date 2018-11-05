using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageRecognition.Classificators.Helpers;
//using TensorFlow;

namespace ImageRecognition.Classificators
{
//    /// <summary>
//    /// Image classificator based on the TensorFlow library.
//    /// </summary>
//    public class TensorFlowClassificator : IImageClassificator
//    {
//        private static string TensorFlowLabelsFilePath { get; } =
//            Path.Combine("resources", "model", "labels.txt");
//
//        private static string TensorFlowModelFilePath { get; } =
//            Path.Combine("resources", "model", "model.pb");

//        /// <summary>
//        /// Classifies a given tensor by using a model and label names.
//        /// </summary>
//        /// <param name="tensor">The tensor so classify. The tensor is usually created from an image.</param>
//        /// <param name="model"></param>
//        /// <param name="labels"></param>
//        /// <returns></returns>
//        private Dictionary<string, float> ClassifyTensor(TFTensor tensor, byte[] model, IReadOnlyList<string> labels)
//        {
//            // Recognition
//            var graph = new TFGraph();
//            graph.Import(model);
//            TFTensor result;
//            using (var session = new TFSession(graph))
//            {
//                var runner = session.GetRunner();
//                runner.AddInput(graph["Placeholder"][0], tensor).Fetch(graph["loss"][0]);
//                var output = runner.Run();
//                result = output[0];
//            }
//
//            // Results
//            var probabilities = ((float[][]) result.GetValue(jagged: true))[0];
//
//            var classificationResults = probabilities
//                .Select((x, i) => new KeyValuePair<string, float>(labels[i], probabilities[i]))
//                .ToDictionary(x => x.Key, x => x.Value);
//            return classificationResults;
//        }
//
//        /// <inheritdoc cref="IImageClassificator.ClassifyImage"/>
//        public Dictionary<string, float> ClassifyImage(byte[] image)
//        {
//            var tensor = TfImageUtil.CreateTensorFromBytes(image);
//
//            var model = File.ReadAllBytes(TensorFlowModelFilePath);
//            var labels = File.ReadAllLines(TensorFlowLabelsFilePath);
//
//            return ClassifyTensor(tensor, model, labels);
//        }
//    }
}