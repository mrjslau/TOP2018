using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using ImageRecognition.Classificators;
using ImageRecognition.OCR;
//using TensorFlow;

namespace ImageRecognition
{
    /// <summary>
    /// Class to showcase or quickly test image recognition capabilities.
    /// 
    /// Usage:
    ///     - Without command arguments: change variable <see cref="TestImageFilePath"/> to point to the image
    /// you want to use for testing.
    ///     - With command arguments: pass a URL to the image that you want to use for testing as the first argument.
    /// Ex.: `ImageRecognition.exe https://images.mentalfloss.com/wp-content/uploads/2008/06/tomato-Salmonella.jpg`
    /// </summary>
    class Program
    {
        public static string TestImageFilePath { get; private set; } =
            Path.Combine("resources", "test", "ocr.jpg");
        
        public static List<string> OcrKeywords = new List<string>
        {
            "grik",
            "kruop",
            "ekolog"
        };
        
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
//            var classificationResults = new TensorFlowClassificator().ClassifyImage(image);
//            var classificationResults = new WebClas
            // OCR
            var text = TesseractOcr.ParseText();
            
            // Print
            Console.WriteLine("------------------------");
            Console.WriteLine("OBJECT CLASSIFICATION");
            Console.WriteLine("------------------------");
            
//            foreach (var classification in classificationResults.OrderByDescending(x => x.Value))
//            {
//                Console.WriteLine($"{classification.Key, 15} => {Math.Round((classification.Value * 100.0), 3)}%");
//            }

            Console.WriteLine("------------------------");
            Console.WriteLine("OCR WEIGHT FINDER RESUTLS");
            Console.WriteLine("------------------------");

            text = text.Trim().Replace("\r\n", " ").Replace("\n", " ");
            var regexWeightFinder = new RegexMetricWeightSubstringFinder();
            var octResult = regexWeightFinder.FindWeightSpecifier(text);
            Console.WriteLine(octResult);
            
            Console.WriteLine("------------------------");
            Console.WriteLine("OCR KEYWORDS");
            Console.WriteLine("------------------------");

            var keywords = text.Split(' ')
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Where(word => OcrKeywords.Any(keyword => word.ToLower().Contains(keyword)));

            foreach (var keyword in keywords)
                Console.WriteLine(keyword);
            
            Console.WriteLine("------------------------");
            stopwatch.Stop();
            Console.WriteLine($"Total time: {stopwatch.Elapsed}");
        }
    }
}