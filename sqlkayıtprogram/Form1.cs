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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True");
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from giris where Kullaniciadi =@Kullaniciadi and Parola = @Parola",baglanti);
            komut.Parameters.AddWithValue("@Kullaniciadi", textBox1.Text);
            komut.Parameters.AddWithValue("@Parola", textBox2.Text);

            SqlDataReader dr = komut.ExecuteReader();
            if(dr.Read())
            {
                Kayıtlar frm = new Kayıtlar();
                frm.Show();
                this.Visible = false;

            }
            else
            {
                MessageBox.Show("Yanlış giriş yaptınız");
            }



            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Çıkış yapmak ister misiniz?", "Çıkış işlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
