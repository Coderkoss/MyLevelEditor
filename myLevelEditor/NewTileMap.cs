using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace myLevelEditor
{
    public partial class NewTileMap : Form
    {
        public bool OKPressed = false;
        public NewTileMap()
        {
            InitializeComponent();
        }

        private void Okbutton_Click(object sender, EventArgs e)
        {
            OKPressed = true;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            OKPressed = false;
            Close();
        }
    }
}
