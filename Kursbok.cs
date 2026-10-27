namespace Lab2Grupp1GUI
{
    public class Kursbok : Annons
    {
        public string ISBN { get; set; }
        public string Författare { get; set; }

        public Kursbok(int id, string titel, int pris, AnnonsSkick skick, Kurs kurs, Student säljare, string isbn, string författare)
            : base(id, titel, pris, skick, kurs, säljare)
        {
            ISBN = isbn;
            Författare = författare;
        }
    }
}