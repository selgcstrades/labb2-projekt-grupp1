using System;

namespace Lab2Grupp1GUI
{
    public abstract class Annons
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public int Pris { get; set; }
        public AnnonsSkick Skick { get; set; }
        public AnnonsStatus Status { get; set; } = AnnonsStatus.TillSalu;
        public Kurs TillhörandeKurs { get; set; }
        public Student Säljare { get; set; }
        public DateTime PubliceringsDatum { get; set; } = DateTime.Now;

        protected Annons(int id, string titel, int pris, AnnonsSkick skick, Kurs kurs, Student säljare)
        {
            Id = id;
            Titel = titel;
            Pris = pris;
            Skick = skick;
            TillhörandeKurs = kurs;
            Säljare = säljare;
        }
    }
}