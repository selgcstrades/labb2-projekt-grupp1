using System;

namespace Lab2Grupp1GUI
{
    public class Affär
    {
        public int Id { get; set; }
        public Student Säljare { get; set; }
        public Student Köpare { get; set; }
        public Annons Annons { get; set; }
        public DateTime Datum { get; set; }

        public Affär(Student säljare, Student köpare, Annons annons)
        {
            Säljare = säljare;
            Köpare = köpare;
            Annons = annons;
            Datum = DateTime.Now;
        }
    }
}