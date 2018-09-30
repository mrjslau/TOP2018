using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using VoicedText.TextVoicers;
using ImageRecognition.Classificators;
using System.Speech.Recognition;
using VoiceRecognition;

namespace ShopLensForms
{
    public partial class ShopLens : Form
    {
        public ShopLens()
        {
            InitializeComponent();
            _captureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in _captureDevices)
            {
                webcam_combobox.Items.Add(device.Name);
            }
            _videoSource = new VideoCaptureDevice();
        }

        private TextVoicer _textVoicer = new TextVoicer();
        private VoiceRecognizer _voiceRecognizer = new VoiceRecognizer();
        private FilterInfoCollection _captureDevices;
        private VideoCaptureDevice _videoSource;
        private IImageClassificator _imageClassifying = new TensorFlowClassificator();

        //Commands and their respective grammar objects.
        private const string WhatIsThisCmd = "What is this";

        //Messages that the text voicer says.
        private const string HelloMessage = "Hello and welcome to ShopLens. It's time to begin your shopping.";
        private const string SeeMessage = "Show me an item and say: what is this. I will identify the item for you.";
        private const string NoLblError = "ERROR: no label names provided to product recognition model.";
        
        private void ShopLens_Load(object sender, EventArgs e)
        {
        }

        /// <summary>This method is called when the Form is shown to the user.</summary> 
        private void ShopLens_Shown(object sender, EventArgs e)
        {
            //Register commands to voice recognizer and register grammar events to methods
            //while the form loads.
            _voiceRecognizer.AddCommand(WhatIsThisCmd, CommandRecognized_WhatIsThis);
            _voiceRecognizer.StartVoiceRecognition();
        }


        /// <summary> Calls method when someone says "what is this" </summary>
        /// <remarks>
        /// This if statement makes sure the <see cref="CAPTURE_Click"/>
        /// is called within the GUI thread. For information see https://stackoverflow.com/a/10170699
        /// </remarks>
        [STAThread]
        private void CommandRecognized_WhatIsThis(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => WhatIsThis_btn_Click(sender, e)));
            }
            else
            {
                WhatIsThis_btn_Click(sender, e);
            }
        }

        private void Start_btn_Click(object sender, EventArgs e)
        {
            if (webcam_combobox.SelectedItem != null)
            {
                _videoSource = new VideoCaptureDevice(_captureDevices[webcam_combobox.SelectedIndex].MonikerString);
                _videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
                _videoSource.Start();
                _textVoicer.SayMessage(SeeMessage);
            }
            else
            {
                MessageBox.Show("Please choose the webcam!");
            }
        }

        /// <summary> This method is called every time the video source receives a new frame. </summary>
        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            live_video.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        

        private void WhatIsThis_btn_Click(object sender, EventArgs e)
        {
            if (live_video.Image != null)
            {
                //This line of code causes trouble when trying to identify items multiple times.
                var image = (Image)live_video.Image.Clone();

                var ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                var classificationResults = _imageClassifying.ClassifyImage(ms.ToArray());

                _textVoicer.SayMessage("This is");

                //Order by probability values and take the first label name.
                string mostConfidentResult = classificationResults.OrderByDescending(x => x.Value).FirstOrDefault().Key;

                if (mostConfidentResult == null) 
                {
                    _textVoicer.SayMessage(NoLblError);
                }
                else
                {
                    _textVoicer.SayMessage(mostConfidentResult);
                }
            }
            else
            {
                MessageBox.Show("The webcam is turned off!");
            }
        }

        private void Exit_btn_Click(object sender, EventArgs e)
        {
            if (_videoSource.IsRunning == true)
            {
                _videoSource.Stop();
            }
            Application.Exit(null);
        }

        private void MyList_btn_Click(object sender, EventArgs e)
        {
            MyListForm ml = new MyListForm();
            ml.ShowDialog();
        }

        private void MyCart_btn_Click(object sender, EventArgs e)
        {
            MyCartForm mc = new MyCartForm();
            mc.ShowDialog();
        }

        private void ShopLens_Load_1(object sender, EventArgs e)
        {

        }
    }
}
