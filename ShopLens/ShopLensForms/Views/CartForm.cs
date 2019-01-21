using ShopLensForms.Controllers;
using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    public partial class CartForm : Form
    {
        private MainController _mainController;

        public CartForm(MainController mainController)
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
