using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using VoicedText;
using ImageRecognition.Classificators;
using VoiceRecognitionWithTextVoicer.VoiceRecognizers;
using ShopLensForms.Controllers;

namespace ShopLensForms
{
    public partial class ShopLens : Form
    {
        private FilterInfoCollection _captureDevices;
        private VideoCaptureDevice _videoSource;
        private MainController _mainController;

        //Messages that the text voicer says.
        private const string HelloMessage = "Hello and welcome to ShopLens. It's time to begin your shopping.";
        private const string SeeMessage = "Show me an item and say: what is this. I will identify the item for you.";
        private const string NoLblError = "ERROR: no label names provided to product recognition model.";

        public ShopLens(MainController mainController)
        {
            Hide();

            InitializeComponent();

            _mainController = mainController;

            _captureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in _captureDevices)
            {
                webcam_combobox.Items.Add(device.Name);
            }
            _videoSource = new VideoCaptureDevice();
        }

        private void ShopLens_Load(object sender, EventArgs e)
        {
        }

        /// <summary>This method is called when the Form is shown to the user.</summary> 
        private void ShopLens_Shown(object sender, EventArgs e)
        {
            _mainController.StartVoiceRecognizer();
        }

        private void Start_btn_Click(object sender, EventArgs e)
        {
            if (webcam_combobox.SelectedItem != null)
            {
                _videoSource = new VideoCaptureDevice(_captureDevices[webcam_combobox.SelectedIndex].MonikerString);
                _videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
                _videoSource.Start();
                _mainController.TextVoicerVoiceMessage(SeeMessage);
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

        public void WhatIsThis_btn_Click(object sender, EventArgs e)
        {
            if (live_video.Image != null)
            {
                var image = (Image)live_video.Image.Clone();

                var ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                var classificationResults = _mainController.ImageClassifyingClassifyImage(ms.ToArray());

                _mainController.TextVoicerVoiceMessage("This is");

                string mostConfidentResult = classificationResults.OrderByDescending(x => x.Value).FirstOrDefault().Key; ;

                if (mostConfidentResult == null) 
                {
                    _mainController.TextVoicerVoiceMessage(NoLblError);
                }
                else
                {
                    _mainController.TextVoicerVoiceMessage(mostConfidentResult);
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
