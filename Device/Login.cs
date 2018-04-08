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
            if (Authenticator.AuthenticateUser(txtUsername.Text, txtPassword.Text))
            {

                VideoPlayer vd = new VideoPlayer();
                vd.Show();
            }

            else
            {
                MessageBox.Show("Invalid username or password","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Login log = new Login();
                log.Show();

            }
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
