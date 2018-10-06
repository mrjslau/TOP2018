using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    public partial class MyListForm : Form
    {
        public MyListForm()
        {
            InitializeComponent();
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void Add_btn_Click(object sender, EventArgs e)
        {
            MyList_listBox.Items.Add(ItemToAdd_textBox.Text);
        }
    }
}
