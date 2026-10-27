namespace Lab2Grupp1GUI
{
    public class Kurs
    {
        public string Kursnamn { get; set; }
        public string Kurskod { get; set; }

        public Kurs(string kursnamn, string kurskod)
        {
            Kursnamn = kursnamn;
            Kurskod = kurskod;
        }
    }
}