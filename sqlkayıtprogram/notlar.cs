using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace sqlkayıtprogram
{
    public partial class notlar : Form
    {

        private string gelenID;

        private Form geriDonusFormu;

        public notlar(string id, Form geriForm)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Geçerli bir kayıt seçilmedi.");
                this.Close();
                return;
            }
            gelenID = id;
            geriDonusFormu = geriForm;
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        void NotlariGoster()
        {
            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();
                SqlDataAdapter vericek = new SqlDataAdapter("SELECT * FROM notlar WHERE OgrId = @OgrId", baglanti);
                vericek.SelectCommand.Parameters.AddWithValue("@OgrId", gelenID);

                DataSet ds = new DataSet();
                vericek.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void notlar_Load(object sender, EventArgs e)
        {
            DersleriComboBoxaYukle();
            NotlariGoster();
            GenelNotOrtalaması();
           
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Ders ismi yazmadınız");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vize notunu yazmadınız");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Final notunu yazmadınız");
                return;
            }
            if (!int.TryParse(textBox2.Text, out int vize))
            {
                MessageBox.Show("Vize notu sayı olmalıdır.");
                return;
            }

            if (!int.TryParse(textBox3.Text, out int final))
            {
                MessageBox.Show("Final notu sayı olmalıdır.");
                return;
            }
            double ortalama = vize * 0.4 + final * 0.6;
            int durum = ortalama >= 60 ? 1 : 0; // Geçtiyse 1, kaldıysa 0
            string harf;
            if (ortalama >= 90) harf = "AA";
            else if (ortalama >= 85) harf = "BA";
            else if (ortalama >= 80) harf = "BB";
            else if (ortalama >= 75) harf = "BC";
            else if (ortalama >= 70) harf = "CB";
            else if (ortalama >= 60) harf = "CC";
            else harf = "FF";

            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();

                // Aynı ders var mı kontrolü
                SqlCommand kmt = new SqlCommand("SELECT * FROM notlar WHERE Ders = @Ders AND OgrId = @OgrId", baglanti);
                kmt.Parameters.AddWithValue("@Ders", comboBox1.Text);
                kmt.Parameters.AddWithValue("@OgrId", gelenID);

                SqlDataReader dr = kmt.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Bu ders zaten kayıtlıdır.");
                    dr.Close();
                    return;
                }
                dr.Close();

                // Yeni kayıt ekleme
                SqlCommand komut = new SqlCommand("INSERT INTO notlar (OgrId, Ders, Vize, Final,Ortalama,Harf) VALUES (@OgrId, @Ders, @Vize, @Final,@Ortalama,@Harf)", baglanti);
                komut.Parameters.AddWithValue("@OgrId", gelenID);
                komut.Parameters.AddWithValue("@Ders", comboBox1.Text);
                komut.Parameters.AddWithValue("@Vize", vize);
                komut.Parameters.AddWithValue("@Final", final);
                komut.Parameters.AddWithValue("@Ortalama", ortalama);
                komut.Parameters.AddWithValue("@Harf", harf);
                komut.ExecuteNonQuery();

                MessageBox.Show("Kayıt başarılı.");
            }

            // DataGridView güncelle
            NotlariGoster();

            // TextBoxları temizle
            
            textBox2.Clear();
            textBox3.Clear();
            GenelNotOrtalaması();

        }


      
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Silinecek ders adını yazmadınız!");
                return;
            }

            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand("DELETE FROM notlar WHERE Ders = @Ders AND OgrId = @OgrId", baglanti);
                komut.Parameters.AddWithValue("@Ders", comboBox1.Text);
                komut.Parameters.AddWithValue("@OgrId", gelenID); // sadece giriş yapan öğrenci için sil

                int etkilenenSatirSayisi = komut.ExecuteNonQuery();

                if (etkilenenSatirSayisi > 0)
                {
                    MessageBox.Show("Ders başarıyla silindi.");
                }
                else
                {
                    MessageBox.Show("Silinecek ders bulunamadı.");
                }
            }

            // DataGridView'i güncelle
            NotlariGoster();
            GenelNotOrtalaması();
            
           



        }

        private void button3_Click(object sender, EventArgs e)
        {
            geriDonusFormu.Show();
            this.Close();
        }




        private void label4_Click(object sender, EventArgs e)
        {

        }



        private void button4_Click(object sender, EventArgs e)
        {
            string secilenDers = comboBox2.Text;

            if (string.IsNullOrEmpty(secilenDers))
            {
                MessageBox.Show("Lütfen bir ders adı girin.");
                return;
            }
            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand("SELECT AVG(CAST(Vize AS FLOAT)), AVG(CAST(Final AS FLOAT)), COUNT(*) FROM Notlar WHERE Ders = @p1 AND Vize IS NOT NULL AND Final IS NOT NULL", baglanti);
                komut.Parameters.AddWithValue("@p1", secilenDers);

                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    double ortVize = dr[0] != DBNull.Value ? Convert.ToDouble(dr[0]) : 0;
                    double ortFinal = dr[1] != DBNull.Value ? Convert.ToDouble(dr[1]) : 0;
                    int ogrenciSayisi = dr[2] != DBNull.Value ? Convert.ToInt32(dr[2]) : 0;

                    textBox5.Text = ortVize.ToString("0.00");  // Ders Ortalaması V:
                    textBox7.Text = ortFinal.ToString("0.00"); // Ders Ortalaması F:
                    textBox6.Text = ogrenciSayisi.ToString(); // Sınava Giren Öğrenci Sayısı
                }

                dr.Close();
              
            }
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        void DersleriComboBoxaYukle()
        {
            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand("SELECT DISTINCT Ders FROM Ders WHERE OgrId = @OgrId", baglanti);
                komut.Parameters.AddWithValue("@OgrId", gelenID);

                SqlDataReader dr = komut.ExecuteReader();
                comboBox1.Items.Clear(); // ComboBox'ı temizle

                while (dr.Read())
                {
                    comboBox1.Items.Add(dr["Ders"].ToString());
                    comboBox2.Items.Add(dr["Ders"].ToString());
                }

                dr.Close();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }


        public void GenelNotOrtalaması()
        {
            double genelOrtalama = 0;
            double toplamKredi = 0;
            double toplamAgirlikliNot = 0;

            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();

                string sorgu = @"
            SELECT d.DersKredi, n.Ortalama 
            FROM Ders d
            INNER JOIN Notlar n ON d.Ders = n.Ders AND d.OgrId = n.OgrId
            WHERE d.OgrId = @OgrId";

                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@OgrId", gelenID);

                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    double kredi = dr["DersKredi"] != DBNull.Value ? Convert.ToDouble(dr["DersKredi"]) : 0;
                    double ortalama = dr["Ortalama"] != DBNull.Value ? Convert.ToDouble(dr["Ortalama"]) : 0;

                    toplamKredi += kredi;
                    toplamAgirlikliNot += kredi * ortalama;
                }
                dr.Close();
            }

            if (toplamKredi > 0)
                genelOrtalama = toplamAgirlikliNot / toplamKredi;
            genelOrtalama = (genelOrtalama * 16) / 500;

            textBox1.Text = genelOrtalama.ToString("0.00");

            int kkredi = 0;
            using (SqlConnection bglnti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                bglnti.Open();

                SqlCommand komut = new SqlCommand("SELECT SUM(DersKredi) FROM Ders WHERE OgrId = @OgrId", bglnti);
                komut.Parameters.AddWithValue("@OgrId", gelenID);

                object sonuc = komut.ExecuteScalar();
                kkredi = sonuc != DBNull.Value ? Convert.ToInt32(sonuc) : 0;
                textBox9.Text = kkredi.ToString();
            }
            int basariliSayisi = 0;
            int basarisizSayisi = 0;

            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand("SELECT Harf FROM Notlar WHERE OgrId = @OgrId", baglanti);
                komut.Parameters.AddWithValue("@OgrId", gelenID);

                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    string harf = dr["Harf"].ToString().Trim().ToUpper();
                    if (harf.StartsWith("FF"))
                  {
                        basarisizSayisi++;
                    }
                    else
                        basariliSayisi++;
                }
                dr.Close();
            }
            textBox8.Text = basariliSayisi.ToString();  // Başarılı
            textBox4.Text = basarisizSayisi.ToString();
            int sorumluders = basariliSayisi + basarisizSayisi;
            textBox10.Text = sorumluders.ToString();

        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
