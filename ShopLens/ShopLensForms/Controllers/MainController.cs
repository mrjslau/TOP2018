using ImageRecognition.Classificators;
using ShopLensApp.VoiceRecognizers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VoicedText.TextVoicers;
using VoiceRecognitionWithTextVoicer.VoiceRecognizers;

namespace ShopLensForms.Controllers
{
    public class MainController
    {
        private IVoicer _textVoicer;
        private IRecognizer _voiceRecognizer;
        private IImageClassificator _imageClassifying;

        private IntroForm _introForm;
        private ShopLens _shopLens;

        private const string whatIsThisCmd = "What is this";

        public MainController()
        {
            _textVoicer = new TextVoicer();
            _voiceRecognizer = new VoiceRecognizer();
            _imageClassifying = new TensorFlowClassificator();

            _introForm = new IntroForm(this);
            _shopLens = new ShopLens(this);
        }

        [STAThread]
        public void StartApp()
        {
            Application.Run(_introForm);
        }

        public void StartVoiceRecognizer()
        {
            //Register commands to voice recognizer and register grammar events to methods
            //while the form loads.
            _voiceRecognizer.AddCommand(whatIsThisCmd, CommandRecognized_WhatIsThis);
            _voiceRecognizer.StartVoiceRecognition();
        }

        /// <summary> Calls method when someone says "what is this" </summary>
        /// <remarks>
        /// This if statement makes sure the <see cref="CAPTURE_Click"/>
        /// is called within the GUI thread. For information see https://stackoverflow.com/a/10170699
        /// </remarks>
        private void CommandRecognized_WhatIsThis(object sender, EventArgs e)
        {
            if (_shopLens.InvokeRequired)
            {
                _shopLens.BeginInvoke(new MethodInvoker(() => _shopLens.WhatIsThis_btn_Click(sender, e)));
            }
            else
            {
                _shopLens.WhatIsThis_btn_Click(sender, e);
            }
        }

        public void TextVoicerVoiceMessage(string seeMessage)
        {
            _textVoicer.SayMessage(seeMessage);
        }

        public Dictionary<string, float> ImageClassifyingClassifyImage(byte[] imgArray)
        {
            return _imageClassifying.ClassifyImage(imgArray);
        }

        public void ShowShopLensForm()
        {
            _shopLens.ShowDialog();
        }

        public string GetMostConfidentResult(Image image)
        {
            var ms = new MemoryStream();

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            var classificationResults = ImageClassifyingClassifyImage(ms.ToArray());

            return classificationResults.OrderByDescending(x => x.Value).FirstOrDefault().Key;
        }
    }
}
