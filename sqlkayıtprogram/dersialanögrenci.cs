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

namespace sqlkayıtprogram
{
    public partial class dersialanögrenci : Form

    {
        private string bolum;

        public dersialanögrenci(string bolum)
        {
            InitializeComponent();
            this.bolum = bolum;
        }

        public dersialanögrenci()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dersialanögrenci_Load(object sender, EventArgs e)
        {
            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand("SELECT * FROM kayit WHERE Bolum = @bolum",baglanti);
                komut.Parameters.AddWithValue("@Bolum", bolum);
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Columns["OgrId"].Visible = false;

            }
        }
    }
    }
