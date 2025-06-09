using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Kullanici kullanici = new Kullanici();
        Console.Title = "İlaç Takip ve Yan Etki Uyarı Sistemi";
        Console.Clear();
        YazdirBaslik("İlaç Takip ve Yan Etki Uyarı Sistemine Hoşgeldiniz!", ConsoleColor.Cyan);
        Console.Write("Lütfen adınızı girin: ");
        kullanici.Ad = Console.ReadLine();

        while (true)
        {
            Console.Clear();
            DozHatirlatici(kullanici);
            YazdirBaslik($"Hoşgeldin, {kullanici.Ad}", ConsoleColor.Cyan);
            YazdirMenu();
            Console.Write("Seçiminiz: ");
            string secim = Console.ReadLine();

            if (secim == "1")
            {
                IlacEkle(kullanici);
            }
            else if (secim == "2")
            {
                IlaclariListele(kullanici, true);
                Console.WriteLine("Devam etmek için bir tuşa basın...");
                Console.ReadKey();
            }
            else if (secim == "3")
            {
                Console.WriteLine("Çıkılıyor...");
                break;
            }
            else if (secim == "4")
            {
                IlacSil(kullanici);
            }
            else if (secim == "5")
            {
                IlacGuncelle(kullanici);
            }
            else
            {
                YazdirUyari("Geçersiz seçim, tekrar deneyin.", ConsoleColor.Yellow);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }

    static void YazdirBaslik(string metin, ConsoleColor renk)
    {
        Console.ForegroundColor = renk;
        Console.WriteLine("+" + new string('-', metin.Length + 2) + "+");
        Console.WriteLine("| " + metin + " |");
        Console.WriteLine("+" + new string('-', metin.Length + 2) + "+");
        Console.ResetColor();
    }

    static void YazdirMenu()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n--- ANA MENÜ ---");
        Console.ResetColor();
        Console.WriteLine("1. İlaç Ekle");
        Console.WriteLine("2. İlaçları Listele");
        Console.WriteLine("3. Çıkış");
        Console.WriteLine("4. İlaç Sil");
        Console.WriteLine("5. İlaç Güncelle");
    }

    static void YazdirUyari(string mesaj, ConsoleColor renk)
    {
        Console.ForegroundColor = renk;
        Console.WriteLine(mesaj);
        Console.ResetColor();
    }

    static void IlacEkle(Kullanici kullanici)
    {
        Console.Clear();
        YazdirBaslik("İlaç Ekle", ConsoleColor.Green);
        Ilac ilac = new Ilac();
        Console.Write("İlaç ismi: ");
        ilac.Isim = Console.ReadLine();
        Console.Write("Doz: ");
        ilac.Doz = Console.ReadLine();
        Console.Write("Kullanma zamanı (yyyy-MM-dd HH:mm): ");
        DateTime zaman;
        if (DateTime.TryParse(Console.ReadLine(), out zaman))
        {
            ilac.KullanmaZamani = zaman;
        }
        else
        {
            ilac.KullanmaZamani = DateTime.Now;
            YazdirUyari("Geçersiz tarih, şimdiki zaman atandı.", ConsoleColor.Yellow);
        }
        Console.Write("Yan etkileri virgülle ayırarak girin: ");
        string yanEtkiInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(yanEtkiInput))
            ilac.YanEtkiListesi.AddRange(yanEtkiInput.Split(","));
        Console.Write("Riskli kombinasyon ilaç isimlerini virgülle ayırarak girin: ");
        string riskliInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(riskliInput))
            ilac.RiskliKombinasyonlar.AddRange(riskliInput.Split(","));

        // Riskli kombinasyon kontrolü
        var mevcutIlacIsimleri = new HashSet<string>(kullanici.Ilaclar.ConvertAll(i => i.Isim.Trim()), StringComparer.OrdinalIgnoreCase);
        var riskliKombinasyonlar = ilac.RiskliKombinasyonlar.ConvertAll(r => r.Trim());
        foreach (var riskli in riskliKombinasyonlar)
        {
            if (mevcutIlacIsimleri.Contains(riskli))
            {
                YazdirUyari($"UYARI: Eklemeye çalıştığınız ilaç, mevcut ilaçlarınızdan '{riskli}' ile riskli bir kombinasyona sahip!", ConsoleColor.Red);
            }
        }

        kullanici.Ilaclar.Add(ilac);
        YazdirUyari("İlaç başarıyla eklendi!", ConsoleColor.Green);
        Console.WriteLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    static void IlaclariListele(Kullanici kullanici, bool detayli = false)
    {
        Console.Clear();
        YazdirBaslik("İlaç Listesi", ConsoleColor.Magenta);
        if (kullanici.Ilaclar.Count == 0)
        {
            YazdirUyari("Hiç ilaç eklenmemiş.", ConsoleColor.Yellow);
            return;
        }
        Console.WriteLine($"{"No",-4} {"İsim",-20} {"Doz",-10} {"Zaman",-20}");
        Console.WriteLine(new string('-', 60));
        int i = 1;
        foreach (var ilac in kullanici.Ilaclar)
        {
            Console.WriteLine($"{i,-4} {ilac.Isim,-20} {ilac.Doz,-10} {ilac.KullanmaZamani,-20:yyyy-MM-dd HH:mm}");
            if (detayli)
            {
                Console.WriteLine($"     Yan Etkiler: {string.Join(", ", ilac.YanEtkiListesi)}");
                Console.WriteLine($"     Riskli Kombinasyonlar: {string.Join(", ", ilac.RiskliKombinasyonlar)}");
            }
            i++;
        }
    }

    static void IlacSil(Kullanici kullanici)
    {
        Console.Clear();
        YazdirBaslik("İlaç Sil", ConsoleColor.Yellow);
        if (kullanici.Ilaclar.Count == 0)
        {
            YazdirUyari("Silinecek ilaç yok.", ConsoleColor.Yellow);
            Console.WriteLine("Devam etmek için bir tuşa basın...");
            Console.ReadKey();
            return;
        }
        IlaclariListele(kullanici);
        Console.Write("Silmek istediğiniz ilacın numarasını girin: ");
        if (int.TryParse(Console.ReadLine(), out int secim) && secim > 0 && secim <= kullanici.Ilaclar.Count)
        {
            var silinen = kullanici.Ilaclar[secim - 1];
            kullanici.Ilaclar.RemoveAt(secim - 1);
            YazdirUyari($"{silinen.Isim} başarıyla silindi.", ConsoleColor.Green);
        }
        else
        {
            YazdirUyari("Geçersiz seçim.", ConsoleColor.Red);
        }
        Console.WriteLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    static void IlacGuncelle(Kullanici kullanici)
    {
        Console.Clear();
        YazdirBaslik("İlaç Güncelle", ConsoleColor.Yellow);
        if (kullanici.Ilaclar.Count == 0)
        {
            YazdirUyari("Güncellenecek ilaç yok.", ConsoleColor.Yellow);
            Console.WriteLine("Devam etmek için bir tuşa basın...");
            Console.ReadKey();
            return;
        }
        IlaclariListele(kullanici);
        Console.Write("Güncellemek istediğiniz ilacın numarasını girin: ");
        if (int.TryParse(Console.ReadLine(), out int secim) && secim > 0 && secim <= kullanici.Ilaclar.Count)
        {
            var ilac = kullanici.Ilaclar[secim - 1];
            Console.WriteLine($"{ilac.Isim} ilacını güncelliyorsunuz. Boş bırakırsanız eski değer korunur.");
            Console.Write($"Yeni isim ({ilac.Isim}): ");
            string yeniIsim = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(yeniIsim)) ilac.Isim = yeniIsim;
            Console.Write($"Yeni doz ({ilac.Doz}): ");
            string yeniDoz = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(yeniDoz)) ilac.Doz = yeniDoz;
            Console.Write($"Yeni kullanma zamanı ({ilac.KullanmaZamani:yyyy-MM-dd HH:mm}): ");
            string yeniZaman = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(yeniZaman) && DateTime.TryParse(yeniZaman, out DateTime dt))
                ilac.KullanmaZamani = dt;
            Console.Write($"Yeni yan etkiler (virgülle, eski: {string.Join(", ", ilac.YanEtkiListesi)}): ");
            string yeniYanEtki = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(yeniYanEtki))
            {
                ilac.YanEtkiListesi = new List<string>(yeniYanEtki.Split(","));
            }
            Console.Write($"Yeni riskli kombinasyonlar (virgülle, eski: {string.Join(", ", ilac.RiskliKombinasyonlar)}): ");
            string yeniRiskli = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(yeniRiskli))
            {
                ilac.RiskliKombinasyonlar = new List<string>(yeniRiskli.Split(","));
            }
            YazdirUyari("İlaç başarıyla güncellendi.", ConsoleColor.Green);
        }
        else
        {
            YazdirUyari("Geçersiz seçim.", ConsoleColor.Red);
        }
        Console.WriteLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    static void DozHatirlatici(Kullanici kullanici)
    {
        DateTime simdi = DateTime.Now;
        bool uyarildi = false;
        foreach (var ilac in kullanici.Ilaclar)
        {
            if (ilac.KullanmaZamani <= simdi)
            {
                YazdirUyari($"HATIRLATMA: {ilac.Isim} ilacını {ilac.KullanmaZamani:yyyy-MM-dd HH:mm} tarihinde/dozunda almanız gerekiyor!", ConsoleColor.Red);
                uyarildi = true;
            }
        }
        if (uyarildi)
        {
            Console.WriteLine();
        }
    }
} 