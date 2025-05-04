using System;
using System.Collections.Generic;
using System .ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sqlkayıtprogram
{
    public partial class Kayıtlar : Form
    {
        
        
        public Kayıtlar()
        {
            InitializeComponent();
        }
        public string SeciliKayitNo;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            int SeciliKayit = dataGridView1.SelectedCells[0].RowIndex;
            SeciliKayitNo = dataGridView1.Rows[SeciliKayit].Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.Rows[SeciliKayit].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[SeciliKayit].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.Rows[SeciliKayit].Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.Rows[SeciliKayit].Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.Rows[SeciliKayit].Cells[5].Value.ToString();
        }

        void goster()
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True");
            baglanti.Open();
            SqlDataAdapter vericek = new SqlDataAdapter("Select * From kayit", baglanti);
            DataSet ds = new DataSet();
            vericek.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();

        }
        private void Kayıtlar_Load(object sender, EventArgs e)
        {
            goster();
         
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            notlar frm = new notlar(SeciliKayitNo,this);
            frm.Show();
            this.Hide();
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            devamsızlık frm = new devamsızlık(SeciliKayitNo,this);
            frm.Show();
            this.Hide();
        }
    }
}
