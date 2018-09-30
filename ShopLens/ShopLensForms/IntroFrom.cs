using System;
using System.Windows.Forms;
using VoicedText;

namespace ShopLensForms
{
    public partial class IntroFrom : Form
    {
        private TextVoicer _textVoicer = new TextVoicer();

        public IntroFrom()
        {
            InitializeComponent();
        }

        private const string HelloMessage = "Hello and welcome to ShopLens. It's time to begin your shopping.";

        //This method is called when the Form is shown to the user.
        private void IntroForm_Shown(object sender, EventArgs e)
        {
            //Greet the user.
            _textVoicer.SayMessage(HelloMessage);
        }

        private void Enter_btn_Click(object sender, EventArgs e)
        {
            ShopLens sp = new ShopLens();
            this.Hide();
            sp.ShowDialog();
        }
    }
}
