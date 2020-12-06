using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CLIENTGUI
{
    public partial class gui : Form
    {
        public gui()
        {
            InitializeComponent();
            client_control1.Init(new wyrd.client());
        }

        private void client_control1_Load(object sender, EventArgs e)
        {

        }
    }
}
