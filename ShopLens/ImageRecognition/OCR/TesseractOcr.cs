using System;
using System.Diagnostics;
using System.IO;

namespace ImageRecognition.OCR
{
    public static class TesseractOcr
    {
        public static string ParseText()
        {
            var solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

            var tesseractPath = solutionDirectory + @"\ImageRecognition\resources\tesseract-master.2072";
            var testFile = solutionDirectory + @"\ImageRecognition\resources\test\ocr.jpg";
            var imageFile = File.ReadAllBytes(Program.TestImageFilePath);
            
            var output = string.Empty;
            var tempOutputFile = Path.GetTempPath() + Guid.NewGuid();
            var tempImageFile = Path.GetTempFileName();

            try
            {
                File.WriteAllBytes(tempImageFile, imageFile);

                var info = new ProcessStartInfo();
                info.WorkingDirectory = tesseractPath;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.UseShellExecute = false;
                info.FileName = "cmd.exe";
                info.Arguments =
                    "/c tesseract.exe " +
                    // Image file.
                    tempImageFile + " " +
                    // Output file (tesseract add '.txt' at the end)
                    tempOutputFile +
                    // Languages.
                    " -l " + string.Join("+", new[]{"lit"}) +
                    " --tessdata-dir " + tesseractPath + "\\tessdata" + 
                    " quiet";

                // Start tesseract.
                var process = Process.Start(info);
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    // Exit code: success.
                    output = File.ReadAllText(tempOutputFile + ".txt");
                }
                else
                {
                    throw new Exception("Error. Tesseract stopped with an error code = " + process.ExitCode);
                }
            }
            finally
            {
                File.Delete(tempImageFile);
                File.Delete(tempOutputFile + ".txt");
            }
            return output;
        }
    }
}