using ImageRecognition.Classificators;
using ShopLensForms.Controllers;
using System.Windows.Forms;
using VoicedText.TextVoicers;
using VoiceRecognitionWithTextVoicer.VoiceRecognizers;

namespace ShopLensForms
{
    static class Program
    {
        static void Main()
        {
            ITextVoicer _textVoicer = new TextVoicerSpeechSynthesizer();
            IVoiceRecognizer _voiceRecognizer = new VoiceRecognizerSpeechRecEngine();
            IImageClassificator _imageClassificator= new TensorFlowClassificator();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IntroForm _introForm = new IntroForm();
            ShopLens _shopLens = new ShopLens();

            var mainController = new MainController(_textVoicer, _voiceRecognizer, _imageClassificator
                , _introForm, _shopLens);

            mainController.StartApp();
        }
    }
}
