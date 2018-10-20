using ShopLensForms.Controllers;
using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    public partial class MyCartForm : Form
    {
        private MainController _mainController;

        public MyCartForm(MainController mainController)
        {
            _mainController = mainController;

            InitializeComponent();
        }

        public void Close_btn_Click(object sender, EventArgs e)
        {
            _mainController.HideForm(this);
        }
    }
}
