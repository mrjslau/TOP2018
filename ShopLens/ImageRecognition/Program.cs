using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using ImageRecognition.Classificators;
using TensorFlow;

namespace ImageRecognition
{
    class Program
    {
        public static string TestImageFilePath { get; private set; } =
            Path.Combine("resources", "test", "1.jpg");
        
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Handle input arguments
            byte[] image;
            
            if (args.Length == 0)
            {
                Console.WriteLine($"Using file: {TestImageFilePath}");                 
                try
                {
                    image = File.ReadAllBytes(TestImageFilePath);
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
                    using (var webClient = new WebClient())
                    {
                        image = webClient.DownloadData(possibleUrl);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error when loading image from URL: {e.Message}");
                    return;
                }
            }

            // Classify
            var classificationResults = new TensorFlowClassificator().ClassifyImage(image);

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