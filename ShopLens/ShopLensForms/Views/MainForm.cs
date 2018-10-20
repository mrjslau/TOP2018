﻿using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ShopLensForms.Controllers;

namespace ShopLensForms
{
    public partial class ShopLens : Form
    {
        private FilterInfoCollection _captureDevices;
        private VideoCaptureDevice _videoSource;

        //Messages that the text voicer says.
        private const string HelloMessage = "Hello and welcome to ShopLens. It's time to begin your shopping.";
        private const string SeeMessage = "Show me an item and say: what is this. I will identify the item for you.";
        private const string NoLblError = "ERROR: no label names provided to product recognition model.";

        public MainController MainController { get; set; }

        public ShopLens()
        {
            Hide();

            InitializeComponent();

            _captureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in _captureDevices)
            {
                webcam_combobox.Items.Add(device.Name);
            }
            _videoSource = new VideoCaptureDevice();
        }

        public void Start_btn_Click(object sender, EventArgs e)
        {
            if (webcam_combobox.SelectedItem != null)
            {
                _videoSource = new VideoCaptureDevice(_captureDevices[webcam_combobox.SelectedIndex].MonikerString);
                _videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
                _videoSource.Start();
                MainController.TextVoicerVoiceMessage(SeeMessage);
            }
            else
            {
                MainController.TextVoicerVoiceMessage("Please choose a webcam!");
            }
        }

        /// <summary> This method is called every time the video source receives a new frame. </summary>
        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            live_video.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        public void WhatIsThis_btn_Click(object sender, EventArgs e)
        {
            var webcamTurnedOff = "The webcam is turned off!";
            var beginningStatement = "This is";
            MainController.ExecuteCommand_WhatIsThis(live_video.Image, webcamTurnedOff,
                beginningStatement ,NoLblError);
        }

        public void Exit_btn_Click(object sender, EventArgs e)
        {
            if (_videoSource.IsRunning == true)
            {
                _videoSource.Stop();
            }
            MainController.StopVoiceRecognizer();
            Application.Exit(null);
        }

        public void MyList_btn_Click(object sender, EventArgs e)
        {
            MainController.ShowForm(MainController._myList);
            MainController.LoadList(MainController._myList.MyList_listBox);
        }

        public void MyCart_btn_Click(object sender, EventArgs e)
        {
            MainController.ShowForm(MainController._myCart);
        }
    }
}
