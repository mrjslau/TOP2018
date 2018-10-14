using ShopLensForms.Controllers;
using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    public partial class MyListForm : Form
    {
        private MainController _mainController;

        public MyListForm(MainController mainController)
        {
            _mainController = mainController;
            InitializeComponent();
        }

        public void Close_btn_Click(object sender, EventArgs e)
        {
            _mainController.HideForm(_mainController._myList);
        }

        public void Add_btn_Click(object sender, EventArgs e)
        {
            _mainController.AddItem();
        }
    }
}
