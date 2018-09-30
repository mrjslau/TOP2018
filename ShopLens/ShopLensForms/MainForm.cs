using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using VoicedText;
using ImageRecognition;
using ImageRecognition.Classificators;

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
        private FilterInfoCollection _captureDevices;
        private VideoCaptureDevice _videoSource;
        private IImageClassifying _imageClassifying = new TensorFlowClassificator();

        //Messages that the text voicer says.
        private const string SeeMessage = "I can see your world now. Show me an item and say: what is this. I will identify the item for you.";

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

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            live_video.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        

        private void WhatIsThis_btn_Click(object sender, EventArgs e)
        {
            if (live_video.Image != null)
            {
                var image = (Bitmap)live_video.Image.Clone();

                var ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

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
    }
}
