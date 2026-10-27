namespace Lab2Grupp1GUI
{
    public class Kompendium : Annons
    {
        public int AntalSidor { get; set; }

        public Kompendium(int id, string titel, int pris, AnnonsSkick skick, Kurs kurs, Student säljare, int antalSidor)
            : base(id, titel, pris, skick, kurs, säljare)
        {
            AntalSidor = antalSidor;
        }
    }
}