using System.Collections.Generic;

namespace Lab2Grupp1GUI
{
    /// <summary>
    /// Representerar systemets databas och hanterar in-memory-listor för användare, annonser, affärer och kurser.
    /// </summary>
    public class Databas
    {
        private readonly List<Användare> _användare = new List<Användare>();
        private readonly List<Annons> _annonser = new List<Annons>();
        private readonly List<Affär> _affärer = new List<Affär>();
        private readonly List<Kurs> _kurser = new List<Kurs>();

        /// <summary>
        /// Initialiserar en ny instans av <see cref="Databas"/> och fyller den med standarddata.
        /// </summary>
        public Databas()
        {
            SeedData();
        }

        /// <summary>
        /// Fyller databasen med fördefinierad testdata för kurser, användare och annonser.
        /// </summary>
        private void SeedData()
        {
            Kurs progTeknik = new Kurs("Programvaruteknik", "DAT123");
            Kurs databasTeknik = new Kurs("Databasdesign", "DAT456");
            _kurser.Add(progTeknik);
            _kurser.Add(databasTeknik);

            Student säljare = new Student(1, "Chad GPThatcher", "chatten@student.se", "lösenord123");
            Student köpare = new Student(2, "Kalle Kihlman", "kalle@kung.se", "lösenord123");
            Administratör admin = new Administratör(3, "Adam Adminsson", "adam@admin.se", "admin123");

            _användare.Add(säljare);
            _användare.Add(köpare);
            _användare.Add(admin);

            var bok1 = new Kursbok(1, "Kursbok i Programvaruteknik", 350, AnnonsSkick.BraSkick, progTeknik, säljare, "978-1234567890", "Sten Svensson");
            var kompendium1 = new Kompendium(2, "Föreläsningsanteckningar Databas", 100, AnnonsSkick.Nyskick, databasTeknik, köpare, 45);

            _annonser.Add(bok1);
            _annonser.Add(kompendium1);

            säljare.EgnaAnnonser.Add(bok1);
            köpare.EgnaAnnonser.Add(kompendium1);
        }

        /// <summary>Hämtar en lista över alla annonser i databasen.</summary>
        public List<Annons> HämtaAllaAnnonser() => _annonser;

        /// <summary>Hämtar en lista över alla användare i databasen.</summary>
        public List<Användare> HämtaAllaAnvändare() => _användare;

        /// <summary>Hämtar en lista över alla genomförda affärer.</summary>
        public List<Affär> HämtaAllaAffärer() => _affärer;

        /// <summary>Hämtar en lista över alla tillgängliga kurser.</summary>
        public List<Kurs> HämtaAllaKurser() => _kurser;

        /// <summary>Lägger till en ny användare i databasen.</summary>
        public void LäggTillAnvändare(Användare användare) => _användare.Add(användare);

        /// <summary>Tar bort en befintlig användare från databasen.</summary>
        public void TaBortAnvändare(Användare användare) => _användare.Remove(användare);

        /// <summary>Ersätter en befintlig användare med en ny.</summary>
        public void ErsättAnvändare(Användare gammalAnvändare, Användare nyAnvändare)
        {
            int index = _användare.IndexOf(gammalAnvändare);
            if (index != -1)
            {
                _användare[index] = nyAnvändare;
            }
        }

        /// <summary>Lägger till en ny annons i databasen.</summary>
        public void LäggTillAnnons(Annons annons) => _annonser.Add(annons);

        /// <summary>Tar bort en annons från databasen.</summary>
        public void TaBortAnnons(Annons annons) => _annonser.Remove(annons);

        /// <summary>Sparar en genomförd affär i databasen.</summary>
        public void SparaAffär(Affär affär) => _affärer.Add(affär);
    }
}