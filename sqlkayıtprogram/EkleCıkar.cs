using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sqlkayıtprogram
{
    public partial class EkleCıkar : Form
    {
        
        private Form GeriDonusFormu;

        public EkleCıkar(Form geriform)
        {
            InitializeComponent();
            GeriDonusFormu = geriform;
        }
       
        private void EkleCıkar_Load(object sender, EventArgs e)
        {
           
            groupBox2.Hide();
            ogoster();
            button9.Hide();
            toolTip1.SetToolTip(button3, "Sadece Öğrenci ID'yi girerek silme işlemi yapınız.");
            toolTip2.SetToolTip(button2, "Öğrenci ID'si sistem tarafından belirlenecektir .Girmenize gerek yoktur.");
            toolTip3.SetToolTip(button6, "Ünvan Ad Soyad bigisi girmeniz yeterlidir.");

        }
        void ogoster()
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True");
            baglanti.Open();
            SqlDataAdapter vericek = new SqlDataAdapter("Select * From kayit", baglanti);
            DataSet ds = new DataSet();
            vericek.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();

        }
        void agoster()
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True");
            baglanti.Open();
            SqlDataAdapter vericek = new SqlDataAdapter("Select * From akademisyen", baglanti);
            DataSet ds = new DataSet();
            vericek.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();

        }
        private void OgrenciEkle(string ad,string soyad,string cinsiyet,string bolum,string stajdurumu)
        {
            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("INSERT INTO kayit (Ad, Soyad,Cinsiyet,Bolum,StajDurumu) VALUES (@Ad, @Soyad, @Cinsiyet,@Bolum,@StajDurumu)",baglanti);
                komut.Parameters.AddWithValue("@Ad", ad);
                komut.Parameters.AddWithValue("@Soyad", soyad);
                komut.Parameters.AddWithValue("@Cinsiyet", cinsiyet);
                komut.Parameters.AddWithValue("@Bolum", bolum);
                komut.Parameters.AddWithValue("@StajDurumu", stajdurumu);
                komut.ExecuteNonQuery();
            }
        }
        private void AkademisyenEkle(string unvan, string ad, string soyad, string ders, string bolum, string telNo,string mail)
        {
            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("INSERT INTO akademisyen (Unvan,Ad,Soyad,Ders,Bolum,TelefonNo,Ogmail) VALUES (@Unvan,@Ad, @Soyad, @Ders,@Bolum,@TelefonNo,@Ogmail)", baglanti);
                komut.Parameters.AddWithValue("@Unvan", unvan);
                komut.Parameters.AddWithValue("@Ad", ad);
                komut.Parameters.AddWithValue("@Soyad", soyad);
                komut.Parameters.AddWithValue("@Ders", ders);
                komut.Parameters.AddWithValue("@Bolum", bolum);
                komut.Parameters.AddWithValue("@TelefonNo",telNo);
                komut.Parameters.AddWithValue("@Ogmail", mail);
                komut.ExecuteNonQuery();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            
            button1.Show();
            groupBox1.Show();
            groupBox2.Hide();
            ogoster();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

       private void OgrenciSil(int id)
{
    using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
    {
        baglanti.Open();
        SqlCommand komut = new SqlCommand("DELETE FROM kayit WHERE OgrId = @OgrId", baglanti);
        komut.Parameters.AddWithValue("@OgrId", id);
        komut.ExecuteNonQuery();
    }
}
        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox6.Text.Trim(), out int id))
            {
                
                DialogResult result = MessageBox.Show("Seçili kaydı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    OgrenciSil(id);
                    MessageBox.Show("Kayıt başarıyla silindi.");
                    ogoster(); // DataGridView'i güncelle
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir kayıt seçin.");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {

            string ad = textBox1.Text;
            string soyad = textBox2.Text;
            string cinsiyet = textBox3.Text;
            string bolum = textBox4.Text;
            string stajdurumu = textBox5.Text;


            OgrenciEkle(ad, soyad, cinsiyet, bolum, stajdurumu);
            MessageBox.Show("Kayıt başarıyla eklendi!");
            ogoster();
    }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)

        {
            
            button1.Hide();
            groupBox1.Hide();
            groupBox2.Show();
            agoster();


        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string unvan =textBox7.Text;
            string ad = textBox8.Text;
            string soyad = textBox9.Text;
            string ders = textBox10.Text;
            string bolum = textBox11.Text;
            string telNo = textBox12.Text;
            string mail = textBox13.Text;


            AkademisyenEkle(unvan, ad, soyad, ders , bolum, telNo,mail);
            MessageBox.Show("Kayıt başarıyla eklendi!");
            agoster();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE FROM akademisyen WHERE Unvan = @Unvan AND Ad = @Ad AND Soyad = @Soyad", baglanti);

                komut.Parameters.AddWithValue("@Unvan", textBox7.Text);
                komut.Parameters.AddWithValue("@Ad", textBox8.Text);
                komut.Parameters.AddWithValue("@Soyad", textBox9.Text);
               

                int silinenKayitSayisi = komut.ExecuteNonQuery();

                if (silinenKayitSayisi > 0)
                {
                    MessageBox.Show("Kayıt başarıyla silindi!");
                    agoster(); // Güncellenmiş tabloyu göster
                }
                else
                {
                    MessageBox.Show("Belirtilen kriterlere uygun kayıt bulunamadı.");
                }
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            button9.Show();
            button8.Hide();
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True");
            baglanti.Open();
           
            SqlDataAdapter arama = new SqlDataAdapter("select * from kayit where Ad like'" + textBox14.Text + "%' order by Ad", baglanti);
            DataSet ds = new DataSet();
            arama.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            GeriDonusFormu.Show();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button9.Hide();
            button8.Show();
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True");
            baglanti.Open();

            SqlDataAdapter arama = new SqlDataAdapter("select * from akademisyen where Ad like'" + textBox14.Text + "%' order by Ad", baglanti);
            DataSet ds = new DataSet();
            arama.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
        }
    }
}
