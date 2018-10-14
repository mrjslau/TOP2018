using ShopLensForms.Controllers;
using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    public partial class IntroForm : Form
    {
        private MainController MainController { get; set; }

        private const string HelloMessage = "Hello and welcome to ShopLens. It's time to begin your shopping.";

        public IntroForm()
        {
            InitializeComponent();
        }

        private void IntroForm_Shown(object sender, EventArgs e)
        {
            MainController.TextVoicerVoiceMessage(HelloMessage);
        }

        public void Enter_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainController.ShowForm(MainController._shopLens);
        }
    }
}
