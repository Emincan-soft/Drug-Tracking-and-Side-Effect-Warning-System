public class Ilac
{
    public string Isim { get; set; }
    public string Doz { get; set; }
    public List<string> YanEtkiListesi { get; set; }
    public List<string> RiskliKombinasyonlar { get; set; }
    public DateTime KullanmaZamani { get; set; }

    public Ilac()
    {
        YanEtkiListesi = new List<string>();
        RiskliKombinasyonlar = new List<string>();
    }
} 