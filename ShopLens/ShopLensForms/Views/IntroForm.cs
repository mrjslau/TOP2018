using ShopLensForms.Controllers;
using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    public partial class IntroForm : Form
    {
        private MainController _mainController;

        private const string HelloMessage = "Hello and welcome to ShopLens. It's time to begin your shopping.";

        public IntroForm(MainController mainController)
        {
            _mainController = mainController;

            InitializeComponent();
        }

        private void IntroForm_Shown(object sender, EventArgs e)
        {
            _mainController.TextVoicerVoiceMessage(HelloMessage);
        }

        public void Enter_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            _mainController.ShowForm(_mainController._shopLens);
        }
    }
}
