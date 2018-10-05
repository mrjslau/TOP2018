using ImageRecognition.Classificators;
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
        /// <inheritdoc cref="ITextVoicer"/>
        private ITextVoicer _textVoicer;

        /// <inheritdoc cref="IVoiceRecognizer"/>
        private IVoiceRecognizer _voiceRecognizer;

        /// <inheritdoc cref="IImageClassificator"/>
        private IImageClassificator _imageClassifying;

        /// <summary>
        /// A windows form that the controller communicates with.
        /// </summary>
        public IntroForm _introForm;

        /// <inheritdoc cref="_introForm"/>
        public ShopLens _shopLens;

        private const string helloCmd = "Hello";
        private const string whatIsThisCmd = "What is this";
        private const string startCmd = "Start";
        private const string exitCmd = "Exit";

        public MainController()
        {
            _textVoicer = new TextVoicerSpeechSynthesizer();
            _voiceRecognizer = new VoiceRecognizerSpeechRecEngine();
            _imageClassifying = new TensorFlowClassificator();

            _introForm = new IntroForm(this);
            _shopLens = new ShopLens(this);
        }

        [STAThread]
        public void StartApp()
        {
            StartVoiceRecognizer();
            Application.Run(_introForm);
        }

        public void StartVoiceRecognizer()
        {
            _voiceRecognizer.AddCommand(helloCmd, CommandRecognized_Hello);
            _voiceRecognizer.AddCommand(whatIsThisCmd, CommandRecognized_WhatIsThis);
            _voiceRecognizer.AddCommand(startCmd, CommandRecognized_Start);
            _voiceRecognizer.AddCommand(exitCmd, CommandRecognized_Exit);
            _voiceRecognizer.StartVoiceRecognition();
        }

        /// <summary> Calls method when someone says a certain command. </summary>
        /// <remarks>
        /// This if statement makes sure the method that must be executed when the specified command is recognized
        /// is called within the GUI thread. For information see https://stackoverflow.com/a/10170699.
        /// </remarks>
        private void CommandRecognized_Hello(object sender, EventArgs e)
        {
            if (_introForm.InvokeRequired)
            {
                _introForm.BeginInvoke(new MethodInvoker(() => _introForm.Enter_btn_Click(sender, e)));
            }
            else
            {
                _introForm.Enter_btn_Click(sender, e);
            }
        }

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
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

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
        private void CommandRecognized_Start(object sender, EventArgs e)
        {
            if (_shopLens.InvokeRequired)
            {
                _shopLens.BeginInvoke(new MethodInvoker(() => _shopLens.Start_btn_Click(sender, e)));
            }
            else
            {
                _shopLens.Start_btn_Click(sender, e);
            }
        }

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
        private void CommandRecognized_Exit(object sender, EventArgs e)
        {
            if (_shopLens.InvokeRequired)
            {
                _shopLens.BeginInvoke(new MethodInvoker(() => _shopLens.Exit_btn_Click(sender, e)));
            }
            else
            {
                _shopLens.Exit_btn_Click(sender, e);
            }
        }

        /// <summary>
        /// Uses a text voicer object to voice a message.
        /// </summary>
        /// <param name="seeMessage">The string of a message to be voiced.</param>
        public void TextVoicerVoiceMessage(string seeMessage)
        {
            _textVoicer.SayMessage(seeMessage);
        }

        /// <summary>
        /// Uses an image classifier object to return a Dictionary of labels and their
        /// probabilities of being in a particular image.
        /// </summary>
        /// <param name="imgArray">A byte array of an image 
        /// to be classified by the image classifier object.</param>
        /// <returns></returns>
        private Dictionary<string, float> ImageClassifyingClassifyImage(byte[] imgArray)
        {
            return _imageClassifying.ClassifyImage(imgArray);
        }

        /// <summary>
        /// Makes a particular Windows form visible to the user.
        /// </summary>
        /// <param name="formToBeShown">The form to be shown to the user.</param>
        public void ShowForm(Form formToBeShown)
        {
            formToBeShown.ShowDialog();
        }

        /// <summary>
        /// Determines what item is most likely to be in a given image.
        /// </summary>
        /// <param name="image">The given image.</param>
        /// <returns>The most confident result of a labeled item being in the given image.</returns>
        public string GetMostConfidentResult(Image image)
        {
            var ms = new MemoryStream();

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            var classificationResults = ImageClassifyingClassifyImage(ms.ToArray());

            return classificationResults.OrderByDescending(x => x.Value).FirstOrDefault().Key;
        }
    }
}
