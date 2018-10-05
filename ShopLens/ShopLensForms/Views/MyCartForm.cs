using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    public partial class MyCartForm : Form
    {
        public MyCartForm()
        {
            InitializeComponent();
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
