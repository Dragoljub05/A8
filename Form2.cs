using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;

namespace A8
{
    public partial class Form2 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=A8;Integrated Security=True");
        int sifra = 0;
        int sifra0 = 0;
        bool izmenaSlike = false;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("select top 1 * from Tip_Antikviteta", con);
                SqlDataReader r = cm.ExecuteReader();
                if (r.Read())
                {
                    label1.Text = "Šifra:     " + r[0];
                    sifra0 = (int)r[0];
                    sifra = sifra0;
                    textBox1.Text = r[1].ToString();
                }
                r.Close();
                ubaci_sliku();
            }
            catch
            {


            }
            if (con.State == ConnectionState.Open) con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("select * from Tip_Antikviteta where TipAntikvitetaID<@sif", con);
                cm.Parameters.AddWithValue("sif", sifra);
                SqlDataReader r = cm.ExecuteReader();
                while (r.Read())
                {
                    label1.Text = "Šifra:     " + r[0];
                    sifra = (int)r[0];
                    textBox1.Text = r[1].ToString();
                }
                r.Close();
                if (sifra == sifra0) button2.Enabled = false;
                button3.Enabled = true;
            }
            catch
            {


            }
            if (con.State == ConnectionState.Open) con.Close();
            ubaci_sliku();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("select * from Tip_Antikviteta where TipAntikvitetaID>@sif", con);
                cm.Parameters.AddWithValue("sif", sifra);
                SqlDataReader r = cm.ExecuteReader();
                if (r.Read())
                {
                    label1.Text = "Šifra:     " + r[0];
                    sifra = (int)r[0];
                    textBox1.Text = r[1].ToString();
                }
                if (!r.Read()) button3.Enabled = false;
                r.Close();
                button2.Enabled = true;
            }
            catch
            {

            }
            if (con.State == ConnectionState.Open) con.Close();
            ubaci_sliku();
        }
        private void ubaci_sliku()
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            try
            {
                pictureBox1.Image = new Bitmap(@"C:\Users\esbz-ucenik03\Desktop\A8\A8\Files\" + sifra + ".png");

            }
            catch
            {
                pictureBox1.Image = new Bitmap(@"C:\Users\esbz-ucenik03\Desktop\A8\A8\Files\no.png");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //izmena slike
            if (izmenaSlike == true)
            {
                pictureBox1.Image.Save(@"C:\Users\esbz-ucenik03\Desktop\A8\A8\Files\" + sifra + ".png");
                izmenaSlike = false;
            }
            //izmena naziva
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("update Tip_Antikviteta set Tip=@tip where TipAntikvitetaID=@sif", con);
                cm.Parameters.AddWithValue("sif", sifra);
                cm.Parameters.AddWithValue("tip", textBox1.Text);
                cm.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);

            }
            if (con.State == ConnectionState.Open) con.Close();
            MessageBox.Show("Podaci su uspešno promenjeni!");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Png|*.png|Jpg|*.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(ofd.FileName);
                izmenaSlike = true;
            }
        }
    }
}
