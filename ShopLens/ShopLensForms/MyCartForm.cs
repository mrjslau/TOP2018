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
    public partial class MyCartForm : Form
    {
        public MyCartForm()
        {
            InitializeComponent();
        }

        private void MyCartForm_Load(object sender, EventArgs e)
        {

        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
