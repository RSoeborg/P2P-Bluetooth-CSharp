using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGreenEnvironment
{
    public partial class FrmSetup : Form
    {

        public event EventHandler NameRecieved;

        public FrmSetup()
        {
            InitializeComponent();

            btnSave.Click += (s, e) => 
            {
                NameRecieved?.Invoke(txtPeerName.Text.Replace(" ", "_"), EventArgs.Empty);
                this.Close();
            };
        }
    }
}
