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
    public partial class VideoPlayer : Form
    {
        public VideoPlayer()
        {
            InitializeComponent();
        }

        private void RenderMovie(int FileId)
        {
            FileStream fstream = new FileStream(Application.StartupPath + "\\Movie.3gp", FileMode.Create, FileAccess.ReadWrite);
            string db = Environment.CurrentDirectory.ToString();
            using (SqlConnection con = new SqlConnection(string.Format(@"Data Source=(LocalDB)\v11.0;AttachDbFilename={0}\inzCloud.mdf;Integrated Security=True", db)))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select TOP 1 [FILE]   FROM [Movies] Where FileId=@FileId", con);
                    cmd.Parameters.AddWithValue("@FileId", FileId);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            byte[] MOvieFile = (byte[])rdr["FILE"];
                            fstream.Write(MOvieFile, 0, MOvieFile.Length);
                          //  pictureBox1.Image = Image.FromStream(fstream);
                            fstream.Flush();
                            fstream.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No movies found in the database");
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



        
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string db = Environment.CurrentDirectory.ToString();

            OpenFileDialog fg = new OpenFileDialog();
            if (fg.ShowDialog() == DialogResult.OK)
            {

                string text = File.ReadAllText(fg.FileName, Encoding.UTF8);
                RenderMovie(Convert.ToInt32(text.Trim()));
                this.textBox1.Text = db + "\\Movie.3gp";
            }

            this.axWindowsMediaPlayer1.URL = textBox1.Text;

        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                axWindowsMediaPlayer1.close();
                string VideoPath = Environment.CurrentDirectory.ToString() + "\\Movie.3gp";
                if (File.Exists(VideoPath))
                {
                    File.Delete(VideoPath);
                }
                Application.Exit();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
       
        }
    }
}
