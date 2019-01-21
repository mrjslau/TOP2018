using ImageRecognition.Classificators;
using ShopLensForms.Controllers;
using System.Threading;
using System.Windows.Forms;
using VoicedText.TextVoicers;
using VoiceRecognitionWithTextVoicer.VoiceRecognizers;

namespace ShopLensForms
{
    static class Program
    {
        static void Main()
        {
            // Locale settings -- uncomment for German language
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de");
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de");

            ITextVoicer _textVoicer = new TextVoicerSpeechSynthesizer();
            IVoiceRecognizer _voiceRecognizer = new VoiceRecognizerSpeechRecEngine();
            IImageClassificator _imageClassificator= new TensorFlowClassificator();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var _introForm = new IntroForm();
            var _shopLens = new ShopLens();

            var mainController = new MainController(_textVoicer, _voiceRecognizer, _imageClassificator
                , _introForm, _shopLens);

            mainController.StartApp();
        }
    }
}
