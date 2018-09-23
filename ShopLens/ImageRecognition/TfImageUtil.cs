// Author: Miguel de Icaza
// Taken and adapted from: https://github.com/migueldeicaza/TensorFlowSharp/blob/master/Examples/ExampleCommon/ImageUtil.cs

using System.IO;
using System.Net;
using TensorFlow;

namespace ImageRecognition
{
    public static class TfImageUtil
    {
        public static TFTensor CreateTensorFromImageUrl(string url, TFDataType destinationDataType = TFDataType.Float)
        {
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                
                return TensorFromBytes(imageBytes, destinationDataType);
            }
        }

        // Convert the image in filename to a Tensor suitable as input to the Inception model.
        public static TFTensor CreateTensorFromImageFile(string file, TFDataType destinationDataType = TFDataType.Float)
        {
            var contents = File.ReadAllBytes(file);

            return TensorFromBytes(contents, destinationDataType);
        }

        public static TFTensor TensorFromBytes(byte[] contents, TFDataType destinationDataType = TFDataType.Float)
        {
            // DecodeJpeg uses a scalar String-valued tensor as input.
            var tensor = TFTensor.CreateString(contents);

            // Construct a graph to normalize the image
            using (var graph =
                ConstructGraphToNormalizeImage(out var input, out var output, destinationDataType))
            {
                // Execute that graph to normalize this one image
                using (var session = new TFSession(graph))
                {
                    var normalized = session.Run(
                        inputs: new[] {input},
                        inputValues: new[] {tensor},
                        outputs: new[] {output});

                    return normalized[0];
                }
            }
        }

        // Additional pointers for using TensorFlow & CustomVision together
        // Python: https://github.com/tensorflow/tensorflow/blob/master/tensorflow/examples/label_image/label_image.py
        // C++: https://github.com/tensorflow/tensorflow/blob/master/tensorflow/examples/label_image/main.cc
        // Java: https://github.com/Azure-Samples/cognitive-services-android-customvision-sample/blob/master/app/src/main/java/demo/tensorflow/org/customvision_sample/MSCognitiveServicesClassifier.java
        private static TFGraph ConstructGraphToNormalizeImage(out TFOutput input, out TFOutput output,
            TFDataType destinationDataType = TFDataType.Float)
        {
            const int w = 227;
            const int h = 227;
            const float scale = 1;

            // Depending on your CustomVision.ai Domain - set appropriate Mean Values (RGB)
            // https://github.com/Azure-Samples/cognitive-services-android-customvision-sample for RGB values (in BGR order)
            var bgrValues = new TFTensor(new float[]
                {104.0f, 117.0f, 123.0f}); // General (Compact) & Landmark (Compact)
            //var bgrValues = new TFTensor(0f); // Retail (Compact)

            var graph = new TFGraph();
            input = graph.Placeholder(TFDataType.String);

            var caster = graph.Cast(graph.DecodeJpeg(contents: input, channels: 3), DstT: TFDataType.Float);
            var dimsExpander = graph.ExpandDims(caster, graph.Const(0, "batch"));
            var resized = graph.ResizeBilinear(dimsExpander, graph.Const(new int[] {h, w}, "size"));
            var resizedMean = graph.Sub(resized, graph.Const(bgrValues, "mean"));
            var normalised = graph.Div(resizedMean, graph.Const(scale));
            output = normalised;
            return graph;
        }
    }
}