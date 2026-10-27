using System.Collections.Generic;

namespace Lab2Grupp1GUI
{
    public class Student : Användare
    {
        public List<Annons> EgnaAnnonser { get; set; } = new List<Annons>();

        public Student(int id, string namn, string epost, string lösenord)
            : base(id, namn, epost, lösenord) { }
    }
}