using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace sqlkayıtprogram
{
    public partial class staj1 : Form
    {
        private string gelenID;

        private Form geriDonusFormu;
        private Dictionary<int, string> raporlar = new Dictionary<int, string>();

        public staj1(string id,Form geriform)
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
        void staj1dokumanokuveyazdir()
        {
            string docPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "StajBilgisi.docx");

            string adi = "", hizmetAlani = "", baslangic = "", bitis = "", sure = "";

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(docPath, false))
            {
                var paragraphs = wordDoc.MainDocumentPart.Document.Body.Elements<Paragraph>();
                foreach (var para in paragraphs)
                {
                    string satir = para.InnerText.Trim();
                    if (satir.StartsWith("Adı:")) adi = satir.Replace("Adı:", "").Trim();
                    else if (satir.StartsWith("Hizmet Alanı:")) hizmetAlani = satir.Replace("Hizmet Alanı:", "").Trim();
                    else if (satir.StartsWith("Staja Başlama tarihi:")) baslangic = satir.Replace("Staja Başlama tarihi:", "").Trim();
                    else if (satir.StartsWith("Staja Bitiş Tarihi:")) bitis = satir.Replace("Staja Bitiş Tarihi:", "").Trim();
                    else if (satir.StartsWith("Staj süresi:")) sure = satir.Replace("Staj süresi:", "").Trim();
                }
            }

            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                conn.Open();
                string sorgu = "INSERT INTO staj1 (Adi, HizmetAlani, BaslangicTarihi, BitisTarihi, StajSuresi) VALUES (@adi, @hizmet, @baslangic, @bitis, @sure)";
                using (SqlCommand cmd = new SqlCommand(sorgu, conn))
                {
                    cmd.Parameters.AddWithValue("@adi", adi);
                    cmd.Parameters.AddWithValue("@hizmet", hizmetAlani);
                    cmd.Parameters.AddWithValue("@baslangic", baslangic);
                    cmd.Parameters.AddWithValue("@bitis", bitis);
                    cmd.Parameters.AddWithValue("@sure", sure);
                    cmd.ExecuteNonQuery();
                }
            }

            // DataGridView'e yükle
            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM staj1 WHERE Adi = @adi", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@adi", adi);

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Visible = false; // ID gizle
            }
        }


        private void staj1_Load(object sender, EventArgs e)
        {
            staj1dokumanokuveyazdir();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rapor = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(rapor))
            {
                MessageBox.Show("Lütfen bir rapor yazın.");
                return;
            }
            using (SqlConnection baglantı = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True")) 
            {
                baglantı.Open();
                string query = "INSERT INTO staj2 (OgrId,RaporMetni, KayitTarihi) VALUES (@OgrId,@rapor, @tarih)";
                SqlCommand cmd = new SqlCommand(query, baglantı);
                cmd.Parameters.AddWithValue("@OgrId", gelenID);
                cmd.Parameters.AddWithValue("@rapor", rapor);
                cmd.Parameters.AddWithValue("@tarih", DateTime.Now);
                cmd.ExecuteNonQuery();
            }

          
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            raporlar.Clear();
            listBox1.Items.Clear();

            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT Id, KayitTarihi, RaporMetni FROM staj2 WHERE OgrId = @OgrId ORDER BY KayitTarihi DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OgrId", gelenID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string tarih = Convert.ToDateTime(reader["KayitTarihi"]).ToString("dd.MM.yyyy");
                            string metin = reader["RaporMetni"].ToString();

                            raporlar[id] = metin;
                            listBox1.Items.Add($"{id} - {tarih} - {metin.Substring(0, Math.Min(metin.Length, 50))}...");
                        }
                    }
                }
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string secilen = listBox1.SelectedItem.ToString();
                int id;
                if (int.TryParse(secilen.Split('-')[0].Trim(), out id) && raporlar.ContainsKey(id))
                {
                    textBox1.Text = raporlar[id];
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            geriDonusFormu.Show();
            this.Close();
        }
    }
    }

