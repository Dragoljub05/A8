using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace A8
{
    public partial class Form3 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=A8;Integrated Security=True");

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            chart1.Series[0].Color = Color.FromArgb(156, Color.Red);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand(@"select g.Grad, count(*) as BrojOtkrica
                                                from Grad g, Lokalitet l, Antikvitet a
                                                where g.GradID=l.GradID and l.LokalitetID=a.LokalitetID
                                                and YEAR(a.DatumPronalaska)>@gd
                                                group by g.Grad
                                                having count(*)>@br", con);
                cm.Parameters.AddWithValue("gd", (int)numericUpDown1.Value);
                cm.Parameters.AddWithValue("br", (int)numericUpDown2.Value);
                DataTable t = new DataTable();
                SqlDataAdapter a = new SqlDataAdapter(cm);
                a.Fill(t);
                dataGridView1.DataSource = t;
                crtaj_grafikon(t);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            if (con.State == ConnectionState.Open) con.Close();
        }
        private void crtaj_grafikon(DataTable t)
        {
            try
            {
                chart1.Series[0].Points.Clear();
                int n = t.Rows.Count;
                int[] brojevi = new int[n];
                string[] gradovi = new string[n];

                for (int i = 0; i < n; i++)
                {
                    brojevi[i] = (int)t.Rows[i][1];
                    gradovi[i] = t.Rows[i][0].ToString();
                    chart1.Series[0].Points.Add(brojevi[i]);
                    chart1.Series[0].Points[i].LegendText = gradovi[i];
                    chart1.Series[0].Points[i].AxisLabel = gradovi[i];
                }

            }
            catch
            {
            }
        }
    }
}
