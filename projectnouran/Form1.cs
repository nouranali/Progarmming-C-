using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;
/*
 * نوران علي محمد اسماعيل جوده
 * سكشن 3  
 *  */
namespace projectnouran
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //initializing dataset, adaptor, sound path and connection
        DataSet ds = new DataSet();
        SqlDataAdapter da1;
        SqlConnection cn;
        string fullpath = "";
        System.Media.SoundPlayer p;  
        private void Form1_Load(object sender, EventArgs e)
        {   //connection string and open connection
            string connstr = "Data Source=(localdb)\\ProjectsV13;Initial Catalog=master;Integrated Security=True";
            cn = new SqlConnection(connstr);
            cn.Open();
            // if the database already exists an exception will be thron so try&catch
            try
            {
                SqlCommand com = new SqlCommand("create database db", cn);
                com.ExecuteNonQuery();
                com.Connection.ChangeDatabase("db");
                com.ExecuteNonQuery();
            }
            catch (Exception)
            {
                cn.ChangeDatabase("db");
              
            }
            //filling dataset
            da1 = new SqlDataAdapter("select * from t1", cn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da1);
            da1.Fill(ds, "t1");
        }
        //button that adds data in the data base
        private void button4_Click(object sender, EventArgs e)
        {
            DataRow r = ds.Tables["t1"].NewRow();
            r["Id"] = int.Parse(textBox5.Text);
            r["edu_level"] = textBox1.Text;
            r["school_name"] = textBox2.Text;
            r["duration_from"] = textBox3.Text;
            r["duration_to"] = textBox4.Text;
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, ImageFormat.Bmp);
            byte[] arr = ms.ToArray();
            r["Image"] = arr;
            byte[] bt = File.ReadAllBytes(fullpath);
            r["sound"] = bt;
            ds.Tables["t1"].Rows.Add(r);
            da1.Update(ds, "t1");
            clear();

        }
        //viewing all data in the datagridview
        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ds.Tables["t1"];
            this.dataGridView1.Columns["sound"].Visible = false;
        }
        //Adding image
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter =
             "Images (.BMP;.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" +
             "All files (.)|*.*";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName, true);
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
  }
        //Viewing data in the form
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataRow r in ds.Tables["t1"].Rows)
            {
                if (Convert.ToInt32(r["Id"]) == int.Parse(textBox5.Text))
                {
                    textBox5.Text = Convert.ToString(r["Id"]);
                    textBox1.Text = Convert.ToString(r["edu_level"]);
                    textBox2.Text = Convert.ToString(r["school_name"]);
                    textBox3.Text = Convert.ToString(r["duration_from"]);
                    textBox4.Text = Convert.ToString(r["duration_to"]);
                    byte[] arr = (byte[])(r["Image"]);
                    MemoryStream ms = new MemoryStream(arr);
                    Bitmap btmp = new Bitmap(ms);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = btmp;
                    byte[] snd_arr = (byte[])(r["sound"]);
                    string name = Path.ChangeExtension(Path.GetRandomFileName(), ".wav");
                    string path = Path.Combine(Path.GetTempPath(), name);
                    File.WriteAllBytes(path, snd_arr);
                    p = new System.Media.SoundPlayer(path);
                    p.Play();

                }
            }
        }
        //adding sound
        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "wav Files|*.wav;";
            openFileDialog2.ShowDialog();
            if (this.openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                fullpath = openFileDialog2.FileName;
            } }
        //exit button
        private void button5_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
            cn.Close();
        }
        //clear all textboxes
        void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            pictureBox1.Image.Dispose();
        } }}
