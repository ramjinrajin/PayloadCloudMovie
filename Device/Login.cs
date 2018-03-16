using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Device
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

      

        private void btnAddDriver_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (txtUsername.Text == "payload_admin@inzenjer.in" && txtPassword.Text == "admin")
            {
                Upload up = new Upload();
                up.ShowDialog();

            }
            else
            {
                VideoPlayer frm = new VideoPlayer();
                frm.Show();
            }
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
