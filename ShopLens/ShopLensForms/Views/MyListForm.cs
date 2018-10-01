using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            this.Hide();
        }

        private void Add_btn_Click(object sender, EventArgs e)
        {
            MyList_listBox.Items.Add(ItemToAdd_textBox.Text);
        }
    }
}
