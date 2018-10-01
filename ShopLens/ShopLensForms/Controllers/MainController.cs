using ImageRecognition.Classificators;
using ShopLensApp.VoiceRecognizers;
using System;
using System.Windows.Forms;
using VoicedText.TextVoicers;
using VoiceRecognitionWithTextVoicer.VoiceRecognizers;

namespace ShopLensForms.Controllers
{
    public class MainController
    {
        private IVoicer _textVoicer = new TextVoicer();
        private IRecognizer _voiceRecognizer = new VoiceRecognizer();
        private IImageClassificator _imageClassifying = new TensorFlowClassificator();
        private IntroFrom introFrom = new IntroFrom();
        private ShopLens shopLens = new ShopLens();

        private const string whatIsThisCmd = "What is this";

        [STAThread]
        public void StartApp()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        
            Application.Run(introFrom);
            Application.Run(shopLens);
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
            if (shopLens.InvokeRequired)
            {
                shopLens.BeginInvoke(new MethodInvoker(() => shopLens.WhatIsThis_btn_Click(sender, e)));
            }
            else
            {
                shopLens.WhatIsThis_btn_Click(sender, e);
            }
        }
    }
}
