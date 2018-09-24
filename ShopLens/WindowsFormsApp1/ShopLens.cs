using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ImageRecognition;
using VoicedText;

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
                comboBox1.Items.Add(Device.Name);
            }
            //comboBox1.SelectedIndex = 0;
            videoSource = new VideoCaptureDevice();
        }

        private void START_Click(object sender, EventArgs e)
        {
            videoSource = new VideoCaptureDevice(CaptureDevices[comboBox1.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
            videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void RESET_Click(object sender, EventArgs e)
        {
            videoSource.Stop();
            pictureBox1.Image = null;
            pictureBox1.Invalidate();
            pictureBox2.Image = null;
            pictureBox2.Invalidate();
        }

        private void PAUSE_Click(object sender, EventArgs e)
        {
            videoSource.Stop();
        }

        private void CAPTURE_Click(object sender, EventArgs e)
        {
            var image = (Bitmap) pictureBox1.Image.Clone();
            pictureBox2.Image = image;
            
            var ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            var classificationResults = Classificator.ClassifyImage(ms.ToArray());

            var textVoicer = new TextVoicer();
            var resultStrings = classificationResults.Select(pair => $"{pair.Key} - {(int)(pair.Value*100)} percent.");
            textVoicer.SayMessage("My estimates on the image are: ");
            foreach (var result in resultStrings)
            {
                textVoicer.SayMessage(result);
                Thread.Sleep(500);
            }
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
    }
}
