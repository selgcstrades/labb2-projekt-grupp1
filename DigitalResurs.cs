namespace Lab2Grupp1GUI
{
    public class DigitalResurs : Annons
    {
        public string Filformat { get; set; }
        public string Nedladdningslänk { get; set; }

        public DigitalResurs(int id, string titel, int pris, AnnonsSkick skick, Kurs kurs, Student säljare, string filformat, string nedladdningslänk)
            : base(id, titel, pris, skick, kurs, säljare)
        {
            Filformat = filformat;
            Nedladdningslänk = nedladdningslänk;
        }
    }
}