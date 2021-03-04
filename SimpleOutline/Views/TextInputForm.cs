using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleOutline.Views
{
    public partial class TextInputForm : Form
    {
        public TextInputForm()
        {
            InitializeComponent();
        }

        public string Message
        {
            get { return txtMessage.Text; }
            set { txtMessage.Text = value; }
        }

        public string InputText
        {
            get { return txtInput.Text; }
            set { txtInput.Text = value; }
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextInputForm_Load(object sender, EventArgs e)
        {
            txtInput.SelectAll();
        }
    }
}
