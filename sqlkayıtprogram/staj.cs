using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;

namespace sqlkayıtprogram
{
    public partial class staj : Form
    {
        private string gelenID;

        private Form geriDonusFormu;
        public staj(string id, Form geriform)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Geçerli bir kayıt seçilmedi.");
                this.Close();
                return;
            }
            gelenID = id;
            geriDonusFormu = geriform;
        }

        private void staj_Load(object sender, EventArgs e)
        {
            
            button3.Hide();
            button4.Hide();
            ogrenciVerileriniGetir();
        }
        private void ogrenciVerileriniGetir()
        {
            if (string.IsNullOrEmpty(gelenID))
            {
                MessageBox.Show("Geçerli bir öğrenci seçilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                conn.Open();

                // Gelen ID'ye göre kayıtlara eriş
                string query = "SELECT OgrId, Ad, Soyad, Bolum, StajDurumu FROM kayit WHERE OgrId = @ogrId";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@ogrId", gelenID);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Veriyi DataGridView'e yükle
                dataGridView1.DataSource = dt;

                // Eğer veri yoksa uyarı ver
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Bu öğrenciye ait kayıt bulunamadı.", "Kayıt Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // DataGridView'de 1. kolonu gizleyelim (Varsa ID kolonu)
                    dataGridView1.Columns[0].Visible = false;
                    string stajDurumu = dt.Rows[0]["StajDurumu"].ToString();
                    if (stajDurumu == "Yok")
                    {
                        button4.Show();
                        button3.Hide();
                    }
                    else if (stajDurumu == "Var")
                    {
                        button3.Show();  // staj1'e geç butonu görünsün
                        button4.Hide();  // güncelleme butonu gizli
                    }
                    else
                    {
                        button3.Hide();
                        button4.Hide();
                    }
                }
            }
        }

        public void WordDosyasiOlusturVeAc()
        {
            string ad = "", soyad = "", bolum = "", stajDurumu = "";

            // gelenID'yi OgrId olarak kullanarak kaydı getir
            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT Ad, Soyad, Bolum, StajDurumu FROM kayit WHERE OgrId = @ogrId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ogrId", gelenID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ad = reader["Ad"].ToString();
                        soyad = reader["Soyad"].ToString();
                        bolum = reader["Bolum"].ToString();
                        stajDurumu = reader["StajDurumu"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Öğrenci bilgisi bulunamadı.");
                        return;
                    }
                }
            }

            // Word dosyasını oluştur ve yaz
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "StajBilgisi.docx");

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = new Body();
                body.Append(new Paragraph(new Run(new Text("ÖĞRENCİ BİLGİLERİ"))));
                body.Append(new Paragraph(new Run(new Text($"Öğrenci ID: {gelenID}"))));
                body.Append(new Paragraph(new Run(new Text($"Ad: {ad}"))));
                body.Append(new Paragraph(new Run(new Text($"Soyad: {soyad}"))));
                body.Append(new Paragraph(new Run(new Text($"Bölüm: {bolum}"))));
                body.Append(new Paragraph(new Run(new Text($"Staj Durumu: {stajDurumu}"))));
                body.Append(new Paragraph(new Run(new Text(" "))));
                body.Append(new Paragraph(new Run(new Text(" "))));
                body.Append(new Paragraph(new Run(new Text(" "))));
                body.Append(new Paragraph(new Run(new Text("STAJ BİLGİLERİ"))));
                body.Append(new Paragraph(new Run(new Text("Adı:"))));
                body.Append(new Paragraph(new Run(new Text("Hizmet Alanı:"))));
                body.Append(new Paragraph(new Run(new Text("Staja Başlama tarihi:"))));
                body.Append(new Paragraph(new Run(new Text("Staja Bitiş Tarihi:"))));
                body.Append(new Paragraph(new Run(new Text("Staj süresi:"))));

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }

            // Oluşturulan dosyayı aç
            System.Diagnostics.Process.Start(path);

        }
        





        void stajdurumunugüncelle()
        {
            if (string.IsNullOrEmpty(gelenID))
            {
                MessageBox.Show("Geçerli bir öğrenci seçilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                conn.Open();

                string kontrolSorgu = "SELECT StajDurumu FROM kayit WHERE OgrId = @ogrId";
                using (SqlCommand kontrolCmd = new SqlCommand(kontrolSorgu, conn))
                {
                    kontrolCmd.Parameters.AddWithValue("@ogrId", gelenID);

                    object mevcutDurum = kontrolCmd.ExecuteScalar();

                    if (mevcutDurum == null)
                    {
                        MessageBox.Show("Bu öğrenci sistemde kayıtlı değil.", "Kayıt Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string durum = mevcutDurum.ToString();

                    if (durum == "Var")
                    {
                        MessageBox.Show("Bu öğrencinin staj durumu zaten VAR. Staj kaydı bulunmaktadır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;

                    }

                    if (durum == "Yok")
                    {
                        // Güncelleme işlemi
                        string guncelleSorgu = "UPDATE kayit SET StajDurumu = 'Var' WHERE OgrId = @ogrId";
                        using (SqlCommand komut = new SqlCommand(guncelleSorgu, conn))
                        {
                            komut.Parameters.AddWithValue("@ogrId", gelenID);
                            komut.ExecuteNonQuery();

                            MessageBox.Show("Staj durumu 'Yok' iken 'Var' olarak güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz staj durumu değeri: " + durum, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    ogrenciVerileriniGetir();
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            WordDosyasiOlusturVeAc();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stajdurumunugüncelle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void button5_Click(object sender, EventArgs e)
        {
            stajdurumunugüncelle();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            staj1 frm = new staj1(gelenID,this);
            frm.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            geriDonusFormu.Show();
            this.Close();
        }
    }
}
