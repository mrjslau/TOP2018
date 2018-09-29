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
        }

        private TextVoicer _textVoicer = new TextVoicer();
        private VoiceRecognizer _voiceRecognizer = new VoiceRecognizer();
        private FilterInfoCollection _captureDevices;
        private VideoCaptureDevice _videoSource;
        private IImageClassifying _imageClassifying = new TensorFlowClassificator();

        //Commands and their respective grammar objects.
        private const string whatIsThisCmd = "What is this";

        //Messages that the text voicer says.
        private const string HelloMessage = "Hello and welcome to ShopLens. It's time to begin your shopping.";
        private const string SeeMessage = "Show me an item and say: what is this. I will identify the item for you.";


        private void ShopLens_Load(object sender, EventArgs e)
        {
        }

        //This method is called when the Form is shown to the user.
        private void ShopLens_Shown(object sender, EventArgs e)
        {
            //Register commands to voice recognizer and register grammar events to methods
            //while the form loads.
            _voiceRecognizer.AddCommand(whatIsThisCmd, CommandRecognized_WhatIsThis);
            _voiceRecognizer.StartVoiceRecognition();

            //Greet the user.
            _textVoicer.SayMessage(HelloMessage);
        }


        //Calls method when someons says "what is this".
        private void CommandRecognized_WhatIsThis(object sender, EventArgs e)
        {
            if (live_video.Image != null)
            {
                Bitmap image = (Bitmap)live_video.Image.Clone();
                capture_picture.Invoke((MethodInvoker) delegate{ capture_picture.Image = image; });

                var ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                image.Dispose();

                var classificationResults = _imageClassifying.ClassifyImage(ms.ToArray());

                var resultStrings = classificationResults.Select(pair => $"{pair.Key} - {(int)(pair.Value * 100)} percent.");
                _textVoicer.SayMessage("My estimates on the image are: ");
                foreach (var result in resultStrings)
                {
                    _textVoicer.SayMessage(result);
                    Thread.Sleep(500);
                }
            }
            else
            {
                MessageBox.Show("The webcam is turned off!");
            }
        }

        private void PRESS_ENTER_TO_START_Click(object sender, EventArgs e)
        {
            MainWindow.Visible = true;
            _captureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in _captureDevices)
            {
                webcam_combobox.Items.Add(device.Name);
            }
            //comboBox1.SelectedIndex = 0;
            _videoSource = new VideoCaptureDevice();
        }

        private void START_Click(object sender, EventArgs e)
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

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            live_video.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void RESET_Click(object sender, EventArgs e)
        {
            _videoSource.Stop();
            live_video.Image = null;
            live_video.Invalidate();
            capture_picture.Image = null;
            capture_picture.Invalidate();
        }

        private void PAUSE_Click(object sender, EventArgs e)
        {
            _videoSource.Stop();
        }

        private void CAPTURE_Click(object sender, EventArgs e)
        {

        }

        private void EXIT_Click(object sender, EventArgs e)
        {
            if (_videoSource.IsRunning == true)
            {
                _videoSource.Stop();
            }
            Application.Exit(null);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ShopLens_Load_1(object sender, EventArgs e)
        {

        }
    }
}
