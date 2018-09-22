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
            Path.Combine("resources", "test", "1.jpg");

        public static string TensorFlowLabelsFilePath { get; private set; } =
            Path.Combine("resources", "model", "labels.txt");

        public static string TensorFlowModelFilePath { get; private set; } =
            Path.Combine("resources", "model", "model.pb");
        
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var model = File.ReadAllBytes(TensorFlowModelFilePath);
            var labels = File.ReadAllLines(TensorFlowLabelsFilePath);

            // Handle input arguments
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

            
            var classificationResults = Classificator.ClassifyTensor(tensor, model, labels);

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