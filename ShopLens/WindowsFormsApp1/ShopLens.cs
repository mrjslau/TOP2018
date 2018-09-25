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
using VoicedText;

namespace WindowsFormsApp
{
    public partial class ShopLens : Form
    {
        public ShopLens()
        {
            InitializeComponent();

        }

        private TextVoicer textVoicer = new TextVoicer();
        private FilterInfoCollection CaptureDevices;
        private VideoCaptureDevice videoSource;

        //Messages that the text voicer says.
        private const string helloMessage = "Hello and welcome to ShopLens. It's time to begin your shopping.";
        private const string seeMessage = "I can see your world now. Show me an item and say: what is this. I will identify the item for you.";

        private void ShopLens_Load(object sender, EventArgs e)
        {
        }

        //This method is called when the Form is shown to the user.
        private void ShopLens_Shown(object sender, EventArgs e)
        {
            //Greet the user.
            textVoicer.SayMessage(helloMessage);
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
            textVoicer.SayMessage(seeMessage);
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
            var image = (Bitmap) live_video.Image.Clone();
            capture_picture.Image = image;
            
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

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ShopLens_Load_1(object sender, EventArgs e)
        {

        }
    }
}
