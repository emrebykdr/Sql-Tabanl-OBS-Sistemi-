    using System.Windows.Forms;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    namespace sqlkayıtprogram
    {
        public partial class devamsızlık : Form


        {
            public string dersialanogrenci { get; set; }
            private string gelenID;
        
            public string SeciliBolum { get; private set; } = "";
       
       
     private Form Geridonusformu;
     
            public devamsızlık(string id,Form geriForm)
            {
                InitializeComponent();
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("Geçerli bir kayıt seçilmedi.");
                    this.Close();
                    return;
                }
                gelenID = id;
                Geridonusformu = geriForm;
            }

            private void devamsızlık_Load(object sender, EventArgs e)

            {
                button5.Hide();
            groupBox4.Hide();
                groupBox2.Hide();
            groupBox3.Hide();
            
                DersleriComboBoxaYukle(comboBox1);
                DevamsizlikGoster();
           
                DersleriComboBoxaYukle(comboBox2);
                BolumleriComboBoxaYukle(comboBox5);
                DersGunuVeSaatiGetir();
            }
            void DevamsizlikGoster()
            {
                using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
                {
                    baglanti.Open();
                    SqlDataAdapter vericek = new SqlDataAdapter("SELECT * FROM devamsızlık WHERE OgrId = @OgrId", baglanti);
                    vericek.SelectCommand.Parameters.AddWithValue("@OgrId", gelenID);

                    DataSet ds = new DataSet();
                    vericek.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Columns[3].Width = 170; // 4. sütunu büyüt
                    dataGridView1.Columns[4].Width = 100;
                    dataGridView1.Columns[5].Width = 100;
                    dataGridView1.Columns[6].Width = 150;
                    dataGridView1.Columns[7].Width = 180;
                    dataGridView1.Columns["Id"].Visible = false;
                    dataGridView1.Columns["DersId"].Visible = false;
                    dataGridView1.Columns["OgrId"].Visible = false;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["DersSaati"].Value != DBNull.Value && row.Cells["YapılanDevamsizlik"].Value != DBNull.Value)
                        {
                            int dersSaati = Convert.ToInt32(row.Cells["DersSaati"].Value);
                            int yapilan = Convert.ToInt32(row.Cells["YapılanDevamsizlik"].Value);

                            int hak = (int)(dersSaati * 0.3);
                            int kalan = hak - yapilan;

                            if (kalan < 0) kalan = 0;

                            row.Cells["KalanDevamsizlik"].Value = kalan;

                            // Durumu metin olarak yaz
                            row.Cells["Durum"].Value = kalan > 0 ? "DEVAMSIZLIKTAN GEÇTİ" : "DEVAMSIZLIKTAN KALDI";
                        }
                    }
                }
            }

            private void DersleriComboBoxaYukle(System.Windows.Forms.ComboBox comboBox)
            {
                using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
                {
                    baglanti.Open();

                    SqlCommand komut = new SqlCommand("SELECT DISTINCT Ders FROM devamsızlık WHERE OgrId = @OgrId", baglanti);
                    komut.Parameters.AddWithValue("@OgrId", gelenID);

                    SqlDataReader dr = komut.ExecuteReader();
                    comboBox.Items.Clear(); // ComboBox'ı temizle

                    while (dr.Read())
                    {
                        comboBox.Items.Add(dr["Ders"].ToString());
                    }

                    dr.Close();
                }
            }
        private void DersleriBolumeGoreYukle(string secilenBolum, System.Windows.Forms.ComboBox comboBox)
        {
            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand("SELECT DISTINCT DersAdi FROM dersprogramlari WHERE Bolum = @Bolum", baglanti);
                komut.Parameters.AddWithValue("@Bolum", secilenBolum);

                SqlDataReader dr = komut.ExecuteReader();
                comboBox.Items.Clear();

                while (dr.Read())
                {
                    comboBox.Items.Add(dr["DersAdi"].ToString());
                }

                dr.Close();
            }
        }


        private void BolumleriComboBoxaYukle(System.Windows.Forms.ComboBox comboBox)
            {
            using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand("SELECT DISTINCT Bolum FROM kayit", baglanti);

                SqlDataReader dr = komut.ExecuteReader();
                comboBox.Items.Clear();

                while (dr.Read())
                {
                    comboBox.Items.Add(dr["Bolum"].ToString());
                }

                dr.Close();
            }


        }

        private void DersGunuVeSaatiGetir()
            {
                using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
                {
                    baglanti.Open();

                    // Günleri getir
                    SqlCommand gunKomut = new SqlCommand("SELECT DISTINCT Gun FROM dersprogramlari", baglanti);
                    SqlDataReader gunReader = gunKomut.ExecuteReader();
                    comboBox4.Items.Clear(); // Gün ComboBox'ı (comboBox5) temizlenir
                    while (gunReader.Read())
                    {
                        comboBox4.Items.Add(gunReader["Gun"].ToString());
                    }
                    gunReader.Close();

                    // Saatleri getir
                    SqlCommand saatKomut = new SqlCommand("SELECT DISTINCT Saat FROM dersprogramlari", baglanti);
                    SqlDataReader saatReader = saatKomut.ExecuteReader();
                    HashSet<string> saatSet = new HashSet<string>();
                    comboBox3.Items.Clear(); // Saat ComboBox'ı (comboBox4) temizlenir
                    while (saatReader.Read())
                    {
                        string saat = saatReader["Saat"].ToString();
                        if (!saatSet.Contains(saat))
                        {
                            saatSet.Add(saat);
                            comboBox3.Items.Add(saat);
                        }
                    }
                    saatReader.Close();
                }
            }
            private void DersProgramiEkle(string Id,string bolum, string gun, string saat, string dersAdi, string derslik, string ogretimElemani)
            {
                using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
                {
                    baglanti.Open();

                    SqlCommand komut = new SqlCommand("INSERT INTO dersprogramlari (Id,Bolum, Gun, Saat, DersAdi, Derslik, OgretimElemani) VALUES (@Id,@Bolum, @Gun, @Saat, @DersAdi, @Derslik, @OgretimElemani)", baglanti);
                    komut.Parameters.AddWithValue("@Id",Id);
                    komut.Parameters.AddWithValue("@Bolum", bolum);
                    komut.Parameters.AddWithValue("@Gun", gun);
                    komut.Parameters.AddWithValue("@Saat", saat);
                    komut.Parameters.AddWithValue("@DersAdi", dersAdi);
                    komut.Parameters.AddWithValue("@Derslik", derslik);
                    komut.Parameters.AddWithValue("@OgretimElemani", ogretimElemani);

                    komut.ExecuteNonQuery();
                }

                FarkliTabloyuGoster(); // Tabloyu güncelle
            }
            private void GuncelDersPrograminiGoster()
            {
                using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
                {
                    baglanti.Open();

                    // Önce öğrencinin bölümünü al
                    using (SqlCommand bolumSorgu = new SqlCommand("SELECT Bolum FROM kayit WHERE OgrId = @OgrId", baglanti))
                    {
                        bolumSorgu.Parameters.AddWithValue("@OgrId", gelenID);
                        object sonuc = bolumSorgu.ExecuteScalar();
                        if (sonuc != null)
                            SeciliBolum = sonuc.ToString();
                        else
                        {
                            MessageBox.Show("Bölüm bilgisi alınamadı.");
                            return;
                        }
                    }

                    // Gün sırasına göre sıralamak için CASE WHEN kullanılıyor
                    string query = @"
                SELECT Gun, Saat, DersAdi, Derslik, OgretimElemani
                FROM dersprogramlari
                WHERE Bolum = @Bolum
                ORDER BY 
                    CASE Gun
                        WHEN 'Pazartesi' THEN 1
                        WHEN 'Salı' THEN 2
                        WHEN 'Çarşamba' THEN 3
                        WHEN 'Perşembe' THEN 4
                        WHEN 'Cuma' THEN 5
                        WHEN 'Cumartesi' THEN 6
                        WHEN 'Pazar' THEN 7
                        ELSE 8
                    END, Saat";

                    SqlDataAdapter vericek = new SqlDataAdapter(query, baglanti);
                    vericek.SelectCommand.Parameters.AddWithValue("@Bolum", SeciliBolum);

                    DataSet ds = new DataSet();
                    vericek.Fill(ds);

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Güncel ders programı bulunamadı.");
                        return;
                    }

                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Columns[0].HeaderText = "Gün";
                    dataGridView1.Columns[1].HeaderText = "Saat";
                    dataGridView1.Columns[2].HeaderText = "Ders Adı";
                    dataGridView1.Columns[3].HeaderText = "Derslik";
                    dataGridView1.Columns[4].HeaderText = "Öğretim Elemanı";
                }
            }



            void FarkliTabloyuGoster()
            {
                using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
                {
                    baglanti.Open();

                    // Önce kayit tablosundan OgrId'ye göre Bölüm bilgisini al
               
                    using (SqlCommand bolumSorgu = new SqlCommand("SELECT Bolum FROM kayit WHERE OgrId = @OgrId", baglanti))
                    {
                        bolumSorgu.Parameters.AddWithValue("@OgrId", gelenID);
                        object sonuc = bolumSorgu.ExecuteScalar();
                        if (sonuc != null)
                            SeciliBolum = sonuc.ToString();
                        else
                        {
                            MessageBox.Show("Bölüm bilgisi bulunamadı.");
                            return;
                        }
                    }

                    // Bölüm bilgisine göre dersProgramlari tablosundan dersleri al
                    string query = "SELECT Gun, Saat, DersAdi, Derslik, OgretimElemani FROM dersprogramlari WHERE Bolum = @Bolum";
                    SqlDataAdapter vericek = new SqlDataAdapter(query, baglanti);
                    vericek.SelectCommand.Parameters.AddWithValue("@Bolum",SeciliBolum);

                    DataSet ds = new DataSet();
                    vericek.Fill(ds);

                    // Ders programı verisi olup olmadığını kontrol et
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Bu bölüme ait ders programı bulunamadı.");
                        return;
                    }

                    // DataGridView'e veriyi bağla
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Columns[1].Width = 120;
                    dataGridView1.Columns[2].Width = 170; // 4. sütunu büyüt
                    dataGridView1.Columns[3].Width = 100;
                    dataGridView1.Columns[4].Width = 195;
                }
            }


            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            private void button3_Click(object sender, EventArgs e)
            {
                Geridonusformu.Show();
                this.Close();
            }

            private void label1_Click(object sender, EventArgs e)
            {

            }

            private void button1_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(comboBox2.Text))
                {
                    MessageBox.Show("Ders ismi seçmediniz.");
                    return;
                }

                if (!int.TryParse(textBox2.Text, out int eklenenDevamsizlik))
                {
                    MessageBox.Show("Geçerli bir devamsızlık sayısı girin.");
                    return;
                }

                string secilenDers = comboBox2.Text.Trim();
                int mevcutDevamsizlik = 0;
                int dersToplamSaat = 0;

                using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
                {
                    baglanti.Open();

                    // Mevcut devamsızlık saatini al
                    SqlCommand select = new SqlCommand("SELECT YapılanDevamsizlik, DersSaati FROM devamsızlık WHERE OgrId = @ogrId AND Ders = @ders", baglanti);
                    select.Parameters.AddWithValue("@ogrId", gelenID);
                    select.Parameters.AddWithValue("@ders", secilenDers);

                    SqlDataReader reader = select.ExecuteReader();
                    if (reader.Read())
                    {
                        mevcutDevamsizlik = reader["YapılanDevamsizlik"] != DBNull.Value ? Convert.ToInt32(reader["YapılanDevamsizlik"]) : 0;
                        dersToplamSaat = reader["DersSaati"] != DBNull.Value ? Convert.ToInt32(reader["DersSaati"]) : 0;
                    }
                    reader.Close();

                    if (mevcutDevamsizlik >= dersToplamSaat)
                    {
                        MessageBox.Show("Daha fazla devamsızlık ekleyemezsiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (mevcutDevamsizlik + eklenenDevamsizlik > dersToplamSaat)
                    {
                        int kalan = dersToplamSaat - mevcutDevamsizlik;
                        MessageBox.Show($"Devamsızlık toplamı {dersToplamSaat} saati geçemez. Kalan: {kalan} saat.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Güncelle
                    SqlCommand guncelle = new SqlCommand(@"
                UPDATE devamsızlık 
                SET YapılanDevamsizlik = ISNULL(YapılanDevamsizlik, 0) + @ekle 
                WHERE OgrId = @ogrId AND Ders = @ders", baglanti);

                    guncelle.Parameters.AddWithValue("@ekle", eklenenDevamsizlik);
                    guncelle.Parameters.AddWithValue("@ogrId", gelenID);
                    guncelle.Parameters.AddWithValue("@ders", secilenDers);

                    guncelle.ExecuteNonQuery();
                }

                MessageBox.Show("Devamsızlık başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DevamsizlikGoster();
            }



            private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

            private void textBox2_TextChanged(object sender, EventArgs e)
            {

            }

            private void button2_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(comboBox2.Text))
                {
                    MessageBox.Show("Ders ismi seçmediniz.");
                    return;
                }

                if (!int.TryParse(textBox2.Text, out int cikarilacakDevamsizlik))
                {
                    MessageBox.Show("Geçerli bir devamsızlık sayısı girin.");
                    return;
                }

                using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
                {
                    baglanti.Open();

                    // Önce mevcut değeri al
                    SqlCommand select = new SqlCommand("SELECT YapılanDevamsizlik FROM devamsızlık WHERE OgrId = @ogrId AND Ders = @ders", baglanti);
                    select.Parameters.AddWithValue("@ogrId", gelenID);
                    select.Parameters.AddWithValue("@ders", comboBox2.Text.Trim());

                    object sonuc = select.ExecuteScalar();
                    int mevcut = sonuc != DBNull.Value ? Convert.ToInt32(sonuc) : 0;

                    int yeniDeger = mevcut - cikarilacakDevamsizlik;
                    if (yeniDeger < 0) yeniDeger = 0;

                    SqlCommand guncelle = new SqlCommand(@"
                UPDATE devamsızlık 
                SET YapılanDevamsizlik = @yeni 
                WHERE OgrId = @ogrId AND Ders = @ders", baglanti);

                    guncelle.Parameters.AddWithValue("@yeni", yeniDeger);
                    guncelle.Parameters.AddWithValue("@ogrId", gelenID);
                    guncelle.Parameters.AddWithValue("@ders", comboBox2.Text.Trim());

                    guncelle.ExecuteNonQuery();
                }

                DevamsizlikGoster();
            }

            private void button4_Click(object sender, EventArgs e)
            {
                FarkliTabloyuGoster();
                button4.Hide();
                button5.Show();
                groupBox1.Hide();
                groupBox2.Show();
            groupBox4.Show();
            
            groupBox3.Show();
            }

        
            private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {

            }

            private void groupBox1_Enter(object sender, EventArgs e)
            {

            }

            private void button5_Click(object sender, EventArgs e)
            {
                button5.Hide();
                button4.Show();
                groupBox2.Hide();
                groupBox1.Show();
            groupBox4.Hide();
           
            groupBox3.Hide();
                DersleriComboBoxaYukle(comboBox1);
                DevamsizlikGoster();


            }

            private void label4_Click(object sender, EventArgs e)
            {

            }

            private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

            private void label5_Click(object sender, EventArgs e)
            {

            }

            private void groupBox2_Enter(object sender, EventArgs e)
            {

            }

            private void button6_Click(object sender, EventArgs e)
            {

                string Id = "25";
                string gun = comboBox4.Text;
                string saat = comboBox3.Text;
                string dersAdi = comboBox1.Text;
                string derslik = textBox1.Text;
                string ogretimElemani = textBox3.Text;

            
                if (string.IsNullOrWhiteSpace(SeciliBolum))
                {
                    MessageBox.Show("Bölüm bilgisi eksik. Lütfen önce tabloyu gösterin.");
                    return;
                }

                DersProgramiEkle(Id,SeciliBolum, gun, saat, dersAdi, derslik, ogretimElemani);
                GuncelDersPrograminiGoster();
            }

            private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

            private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

            private void button7_Click(object sender, EventArgs e)
            {
                string gun = comboBox4.Text;
                string saat = comboBox3.Text;
                string dersAdi = comboBox1.Text;

                if (string.IsNullOrWhiteSpace(gun) || string.IsNullOrWhiteSpace(saat) || string.IsNullOrWhiteSpace(dersAdi))
                {
                    MessageBox.Show("Lütfen silinecek dersin Gün, Saat ve Adını seçin.");
                    return;
                }

                using (SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True"))
                {
                    baglanti.Open();

                    SqlCommand silKomutu = new SqlCommand(@"
                DELETE FROM dersprogramlari 
                WHERE Gun = @gun AND Saat = @saat AND DersAdi = @dersAdi AND Bolum = @bolum", baglanti);

                    silKomutu.Parameters.AddWithValue("@gun", gun);
                    silKomutu.Parameters.AddWithValue("@saat", saat);
                    silKomutu.Parameters.AddWithValue("@dersAdi", dersAdi);
                    silKomutu.Parameters.AddWithValue("@bolum", SeciliBolum);

                    int etkilenenSatir = silKomutu.ExecuteNonQuery();

                    if (etkilenenSatir > 0)
                    {
                        MessageBox.Show("Ders başarıyla silindi.");
                    }
                    else
                    {
                        MessageBox.Show("Silinecek ders bulunamadı.");
                    }

                    GuncelDersPrograminiGoster();
                }

            }

            private void label7_Click(object sender, EventArgs e)
            {

            }

            private void textBox3_TextChanged(object sender, EventArgs e)
            {

            }

            private void textBox1_TextChanged(object sender, EventArgs e)
            {

            }

            private void groupBox3_Enter(object sender, EventArgs e)
            {

            }

            private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
            {

            }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

            private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
            {
                string secilenBolum = comboBox5.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(secilenBolum))
                {
                    DersleriBolumeGoreYukle(secilenBolum, comboBox6);
                
                }
            }
            private void button8_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(comboBox5.Text))
                {
                    MessageBox.Show("Lütfen bir bölüm seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string secilenBolum = comboBox5.Text.Trim();
                dersialanögrenci ogrenciFormu = new dersialanögrenci(secilenBolum);
                ogrenciFormu.Show();
            }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
       private void DersAra()
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True");
            baglanti.Open();
            SqlDataAdapter arama = new SqlDataAdapter("select * from dersprogramlari where DersAdi like'%" + textBox4.Text + "%'and Bolum= '"+SeciliBolum +"' order by DersAdi", baglanti);
            DataSet ds = new DataSet();
            arama.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
        }
        private void GunAra()
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db1.mdf;Integrated Security=True");
            baglanti.Open();
            SqlDataAdapter arama = new SqlDataAdapter("select * from dersprogramlari where Gun like'" + textBox5.Text + "%'and Bolum= '" + SeciliBolum + "' order by Gun", baglanti);
            DataSet ds = new DataSet();
            arama.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DersAra();
            //GunAra();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            GunAra();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
    }
