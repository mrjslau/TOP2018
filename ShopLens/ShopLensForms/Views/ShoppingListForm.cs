using ShopLensForms.Controllers;
using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    public partial class ShoppingListForm : Form
    {
        private MainController _mainController;

        public ShoppingListForm(MainController mainController)
        {
            _mainController = mainController;
            InitializeComponent();
        }

        public void Close_btn_Click(object sender, EventArgs e)
        {
            _mainController.HideForm(_mainController.ShoppingList);
        }

        public void Add_btn_Click(object sender, EventArgs e)
        {
            _mainController.AddItem();
        }
    }
}
