using ShopLensForms.Controllers;
using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    public partial class IntroForm : Form
    {
        public MainController MainController { get; set; }

        private readonly string _helloMessage = ShopLensApp.GlobalStrings.HelloMessage;

        public IntroForm()
        {
            InitializeComponent();
        }

        private void IntroForm_Shown(object sender, EventArgs e)
        {
            MainController.TextVoicerVoiceMessage(_helloMessage);
        }

        public void Enter_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainController.ShowForm(MainController._shopLens);
        }
    }
}
