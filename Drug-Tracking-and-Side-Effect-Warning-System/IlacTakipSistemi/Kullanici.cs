public class Kullanici
{
    public string Ad { get; set; }
    public List<Ilac> Ilaclar { get; set; }

    public Kullanici()
    {
        Ilaclar = new List<Ilac>();
    }
} 