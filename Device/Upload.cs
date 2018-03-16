using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Device
{
    public partial class Upload : Form
    {
        public Upload()
        {
            InitializeComponent();
        }

        private void RenderImage()
        {
            FileStream fstream = new FileStream(Application.StartupPath + "\\Movie.3gp", FileMode.Create, FileAccess.ReadWrite);
            string db = Environment.CurrentDirectory.ToString();
            using (SqlConnection con = new SqlConnection(string.Format(@"Data Source=(LocalDB)\v11.0;AttachDbFilename={0}\inzCloud.mdf;Integrated Security=True", db)))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select TOP 1  [IMAGE] FROM  [IMAGES] Where Code=@Code", con);
                    cmd.Parameters.AddWithValue("@Code", 1);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            byte[] img = (byte[])rdr["IMAGE"];
                            fstream.Write(img, 0, img.Length);
                          //  pictureBox1.Image = Image.FromStream(fstream);
                            fstream.Flush();
                            fstream.Close();
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }

            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fg = new OpenFileDialog();
            if (DialogResult.OK == fg.ShowDialog())
            {
                textBox1.Text = fg.FileName;
                if (File.Exists(textBox1.Text))
                {
                    
                }
            }

            byte[] MovieFile = new byte[] { };
            if (File.Exists(textBox1.Text))
            {
                StreamReader strImg = new StreamReader(textBox1.Text);
                MovieFile = new byte[File.ReadAllBytes(textBox1.Text).Length];
                MovieFile = File.ReadAllBytes(textBox1.Text);
                strImg.Close();
            }



            string db = Environment.CurrentDirectory.ToString();
            using (SqlConnection con = new SqlConnection(string.Format(@"Data Source=(LocalDB)\v11.0;AttachDbFilename={0}\inzCloud.mdf;Integrated Security=True", db)))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Movies ([File],FileId) VALUES (@File,@FileId)", con);
                    cmd.Parameters.AddWithValue("@File", MovieFile);
                    cmd.Parameters.AddWithValue("@FileId", textBox2.Text);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }

            }
        }
    }
}
