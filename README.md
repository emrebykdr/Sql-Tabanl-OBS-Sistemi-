
# Öğrenci Bilgi Sistemi - C# WinForms

Bu proje, bir okulun öğrenci bilgilerini yönetmek için C# WinForms kullanılarak geliştirilmiş bir masaüstü uygulamasıdır. Uygulama, öğrencilerin kişisel bilgilerini, staj durumlarını, ders programlarını ve devamsızlık bilgilerini yönetmeyi amaçlar.

##  Genel Görünüm

![Uygulama Ekranı](./Ekran görüntüsü 2025-05-08 113542.png)

##  Özellikler

- Öğrenci bilgilerini (Ad, Soyad, Cinsiyet, Bölüm, Staj Durumu) listeleme
- Öğrenci kayıt ekleme
- Staj bilgilerini takip etme
- Devamsızlık ve ders programı görüntüleme
- Not bilgilerine erişim

## 🗂️ Kullanılan Teknolojiler

- **C#** - Uygulama dili
- **WinForms** - Grafik arayüz
- **SQL Server** - Veritabanı yönetimi (eğer entegreyse)
- **DataGridView** - Öğrenci listesinin gösterimi

lum ve Kullanım

1. Bu projeyi klonlayın:
   ```bash
   https://github.com/emrebykdr/Sql-Tabanl-OBS-Sistemi-

2. Visual Studio ile açın.

3. Gerekirse bağlantı stringini kendi veritabanınıza göre güncelleyin.

4. Projeyi derleyip çalıştırın (F5).

## 📁 Proje Yapısı

- `Form1.cs` – Ana form ve öğrenci listeleme
- `staj1.cs` – Staj bilgileri formu
- `devamsizlik.cs` – Devamsızlık ve ders programı formu
- `notlar.cs` – Not görüntüleme formu
- `sql` – (Varsa) Veritabanı bağlantısı ve sorgular

## 📌 Notlar

- Görsel kullanıcı arayüzü, kullanıcı dostu bir deneyim sağlayacak şekilde düzenlenmiştir.
- Uygulamada form kontrolleri üzerinden manuel veri girişleri ve güncellemeler yapılabilir.

