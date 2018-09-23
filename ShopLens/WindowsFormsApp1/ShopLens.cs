using System;
using System.Drawing;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;

namespace WindowsFormsApp
{
    public partial class ShopLens : Form
    {
        public ShopLens()
        {
            InitializeComponent();

        }

        private FilterInfoCollection CaptureDevices;
        private VideoCaptureDevice videoSource;

        private void ShopLens_Load(object sender, EventArgs e)
        {
            
        }

        private void PRESS_ENTER_TO_START_Click(object sender, EventArgs e)
        {
            MainWindow.Visible = true;
            CaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevices)
            {
                webcam_combobox.Items.Add(Device.Name);
            }
            //comboBox1.SelectedIndex = 0;
            videoSource = new VideoCaptureDevice();
        }

        private void START_Click(object sender, EventArgs e)
        {
            videoSource = new VideoCaptureDevice(CaptureDevices[webcam_combobox.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
            videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            live_video.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void RESET_Click(object sender, EventArgs e)
        {
            videoSource.Stop();
            live_video.Image = null;
            live_video.Invalidate();
            capture_picture.Image = null;
            capture_picture.Invalidate();
        }

        private void PAUSE_Click(object sender, EventArgs e)
        {
            videoSource.Stop();
        }

        private void CAPTURE_Click(object sender, EventArgs e)
        {
            capture_picture.Image = (Bitmap)live_video.Image.Clone();
        }

        private void EXIT_Click(object sender, EventArgs e)
        {
            if(videoSource.IsRunning == true)
            {
                videoSource.Stop();
            }
            Application.Exit(null);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ShopLens_Load_1(object sender, EventArgs e)
        {

        }
    }
}
