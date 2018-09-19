using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TensorFlowSharpCore;

namespace ImageRecognition
{
    class Program
    {
        public static string TestImageFilePath { get; private set; } =
            "C:\\Users\\Edvard\\RiderProjects\\TOP2018\\ShopLens\\ImageRecognition\\resources\\train\\tomato\\1.jpg";

        public static string TensorFlowLabelsFilePath { get; private set; } =
            "C:\\Users\\Edvard\\RiderProjects\\TOP2018\\ShopLens\\ImageRecognition\\resources\\model\\labels.txt";

        public static string TensorFlowModelFilePath { get; private set; } =
            "C:\\Users\\Edvard\\RiderProjects\\TOP2018\\ShopLens\\ImageRecognition\\resources\\model\\model.pb";
        
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var graph = new TFGraph();
            var model = File.ReadAllBytes(TensorFlowModelFilePath);
            var labels = File.ReadAllLines(TensorFlowLabelsFilePath);

            graph.Import(model);

            var best = 0.0f;

            Dictionary<string, float> classificationResults;
            using (var session = new TFSession(graph))
            {
                TFTensor tensor;
                if (args.Length == 0)
                {
                    Console.WriteLine($"Using file: {TestImageFilePath}");                 
                    try
                    {
                        tensor = TfImageUtil.CreateTensorFromImageFile(TestImageFilePath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error when loading image: {e.Message}");
                        return;
                    }
                }
                else
                {
                    var possibleUrl = args[0];
                    Console.WriteLine($"Using url from args: {possibleUrl}");
                    try
                    {
                        tensor = TfImageUtil.CreateTensorFromImageUrl(possibleUrl);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error when loading image from URL: {e.Message}");
                        return;
                    }
                }

                var runner = session.GetRunner();
                runner.AddInput(graph["Placeholder"][0], tensor).Fetch(graph["loss"][0]);
                var output = runner.Run();
                var result = output[0];

                // Choose most probable class 
                var probabilities = ((float[][]) result.GetValue(jagged: true))[0];
                for (var i = 0; i < probabilities.Length; i++)
                {
                    if (probabilities[i] > best)
                    {
                        best = probabilities[i];
                    }
                }

                classificationResults =
                    probabilities
                        .Select((x, i) => new KeyValuePair<string, float>(labels[i], probabilities[i]))
                        .ToDictionary(x => x.Key, x => x.Value);
            }

            // Print
            stopwatch.Stop();
            foreach (var classification in classificationResults.OrderByDescending(x => x.Value))
            {
                Console.WriteLine($"{classification.Key, 15} => {Math.Round((classification.Value * 100.0), 3)}%");
            }
            Console.WriteLine($"Total time: {stopwatch.Elapsed}");
        }
    }
}