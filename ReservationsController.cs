using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2Grupp1GUI
{
    /// <summary>
    /// Huvudkontroller som hanterar affärslogik, användare, annonser och databasinteraktioner.
    /// </summary>
    public class ReservationsController
    {
        private readonly Databas _databas = new Databas();

        /// <summary>Hämtar en lista över alla tillgängliga annonser till salu.</summary>
        public List<Annons> TillgängligaAnnonser => _databas.HämtaAllaAnnonser().Where(a => a.Status == AnnonsStatus.TillSalu).ToList();

        /// <summary>Hämtar en lista över alla annonser i systemet.</summary>
        public List<Annons> AllaAnnonser => _databas.HämtaAllaAnnonser();

        /// <summary>Hämtar en lista över alla genomförda affärer.</summary>
        public List<Affär> GenomfördaAffärer => _databas.HämtaAllaAffärer();

        /// <summary>Hämtar en lista över systemets samtliga användare.</summary>
        public List<Användare> AllaAnvändare => _databas.HämtaAllaAnvändare();

        /// <summary>Hämtar en lista över alla tillgängliga kurser.</summary>
        public List<Kurs> AllaKurser => _databas.HämtaAllaKurser();

        /// <summary>
        /// Loggar in en användare baserat på e-post och lösenord.
        /// </summary>
        public Användare LoggaIn(string epost, string lösenord)
        {
            return _databas.HämtaAllaAnvändare()
                .FirstOrDefault(u => u.Epost.Equals(epost, StringComparison.OrdinalIgnoreCase) && u.Lösenord == lösenord);
        }

        /// <summary>
        /// Registrerar en ny student i systemet.
        /// </summary>
        public Student RegistreraStudent(string namn, string epost, string lösenord)
        {
            if (string.IsNullOrWhiteSpace(namn) || string.IsNullOrWhiteSpace(epost) || string.IsNullOrWhiteSpace(lösenord))
            {
                throw new Exception("Alla fält måste fyllas i.");
            }

            bool epostFinns = _databas.HämtaAllaAnvändare().Any(u => u.Epost.Equals(epost, StringComparison.OrdinalIgnoreCase));
            if (epostFinns)
            {
                throw new Exception("En användare med denna e-postadress finns redan registrerad.");
            }

            int nyttId = _databas.HämtaAllaAnvändare().Count + 1;
            Student nyStudent = new Student(nyttId, namn, epost, lösenord);

            _databas.LäggTillAnvändare(nyStudent);
            return nyStudent;
        }

        /// <summary>
        /// Skapar en ny annons för en kursbok.
        /// </summary>
        public void SkapaAnnons(Student säljare, string titel, int pris, AnnonsSkick skick, string kursNamn)
        {
            if (string.IsNullOrWhiteSpace(titel) || pris <= 0 || string.IsNullOrWhiteSpace(kursNamn))
            {
                throw new Exception("Fyll i giltig titel, pris och kursnamn.");
            }

            Kurs kurs = _databas.HämtaAllaKurser().FirstOrDefault(k => k.Kursnamn.Equals(kursNamn.Trim(), StringComparison.OrdinalIgnoreCase));
            if (kurs == null)
            {
                string kurskod = "KURS" + (_databas.HämtaAllaKurser().Count + 1);
                kurs = new Kurs(kurskod, kursNamn.Trim());
                _databas.HämtaAllaKurser().Add(kurs);
            }

            int nyttId = _databas.HämtaAllaAnnonser().Any() ? _databas.HämtaAllaAnnonser().Max(a => a.Id) + 1 : 1;

            Kursbok nyBok = new Kursbok(nyttId, titel, pris, skick, kurs, säljare, "", "");

            _databas.LäggTillAnnons(nyBok);
            säljare.EgnaAnnonser.Add(nyBok);
        }

        /// <summary>
        /// Reserverar en specifik annons för en köpande student.
        /// </summary>
        public void ReserveraAnnons(Annons annons, Student köpare)
        {
            if (annons.Status != AnnonsStatus.TillSalu)
            {
                throw new Exception("Annonsen är inte tillgänglig för reservation.");
            }

            if (annons.Säljare.Id == köpare.Id)
            {
                throw new Exception("Du kan inte reservera din egen annons.");
            }

            annons.Status = AnnonsStatus.Reserverad;
        }

        /// <summary>
        /// Bekräftar köpet av en reserverad annons och skapar en affär.
        /// </summary>
        public Affär BekräftaKöp(Annons annons, Student köpare)
        {
            if (annons.Status == AnnonsStatus.Såld)
            {
                throw new Exception("Annonsen är redan såld.");
            }

            annons.Status = AnnonsStatus.Såld;

            Affär kvitto = new Affär(annons.Säljare, köpare, annons);
            _databas.SparaAffär(kvitto);

            return kvitto;
        }

        /// <summary>
        /// Söker efter annonser baserat på sökord (titel eller ID) och valfri kurs.
        /// </summary>
        public List<Annons> SökAnnonser(string sökord, Kurs kurs = null)
        {
            var query = _databas.HämtaAllaAnnonser().AsQueryable();

            if (!string.IsNullOrEmpty(sökord))
            {
                query = query.Where(a => a.Titel.Contains(sökord, StringComparison.OrdinalIgnoreCase) || a.Id.ToString() == sökord.Trim());
            }

            if (kurs != null)
            {
                query = query.Where(a => a.TillhörandeKurs != null && a.TillhörandeKurs.Kurskod == kurs.Kurskod);
            }

            return query.ToList();
        }

        /// <summary>
        /// Tar bort en annons med administratörsbehörighet.
        /// </summary>
        public void AdminTaBortAnnons(Administratör admin, Annons annons)
        {
            ValideraAdmin(admin);
            _databas.TaBortAnnons(annons);
        }

        /// <summary>
        /// Redigerar en befintlig annons med administratörsbehörighet.
        /// </summary>
        public void AdminRedigeraAnnons(Administratör admin, Annons annons, string nyTitel, int nyttPris, AnnonsSkick nyttSkick)
        {
            ValideraAdmin(admin);
            if (!string.IsNullOrWhiteSpace(nyTitel)) annons.Titel = nyTitel;
            if (nyttPris > 0) annons.Pris = nyttPris;
            annons.Skick = nyttSkick;
        }

        /// <summary>
        /// Skapar en ny användare (student eller administratör) som administratör.
        /// </summary>
        public void AdminSkapaAnvändare(Administratör admin, string namn, string epost, string lösenord, bool ärAdmin)
        {
            ValideraAdmin(admin);

            if (string.IsNullOrWhiteSpace(namn) || string.IsNullOrWhiteSpace(epost) || string.IsNullOrWhiteSpace(lösenord))
            {
                throw new Exception("Alla fält måste fyllas i.");
            }

            if (_databas.HämtaAllaAnvändare().Any(u => u.Epost.Equals(epost, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("E-postadressen är redan registrerad.");
            }

            int nyttId = _databas.HämtaAllaAnvändare().Count + 1;
            Användare nyAnvändare = ärAdmin
                ? (Användare)new Administratör(nyttId, namn, epost, lösenord)
                : new Student(nyttId, namn, epost, lösenord);

            _databas.LäggTillAnvändare(nyAnvändare);
        }

        /// <summary>
        /// Uppdaterar uppgifter och roll för en befintlig användare som administratör.
        /// </summary>
        public void AdminRedigeraAnvändare(Administratör admin, Användare målAnvändare, string nyttNamn, string nyEpost, bool blirAdmin)
        {
            ValideraAdmin(admin);

            if (string.IsNullOrWhiteSpace(nyttNamn) || string.IsNullOrWhiteSpace(nyEpost))
            {
                throw new Exception("Namn och e-post får inte vara tomma.");
            }

            bool epostUpptagen = _databas.HämtaAllaAnvändare().Any(u => u.Id != målAnvändare.Id && u.Epost.Equals(nyEpost, StringComparison.OrdinalIgnoreCase));
            if (epostUpptagen)
            {
                throw new Exception("E-postadressen används redan av en annan användare.");
            }

            målAnvändare.Namn = nyttNamn;
            målAnvändare.Epost = nyEpost;

            bool ärAdminNu = målAnvändare is Administratör;
            if (ärAdminNu != blirAdmin)
            {
                if (admin.Id == målAnvändare.Id && !blirAdmin)
                {
                    throw new Exception("Du kan inte ta bort din egen administratörsroll.");
                }

                Användare nyAnvändare = blirAdmin
                    ? (Användare)new Administratör(målAnvändare.Id, nyttNamn, nyEpost, målAnvändare.Lösenord)
                    : new Student(målAnvändare.Id, nyttNamn, nyEpost, målAnvändare.Lösenord);

                _databas.ErsättAnvändare(målAnvändare, nyAnvändare);
            }
        }

        /// <summary>
        /// Tar bort en användare från systemet som administratör.
        /// </summary>
        public void AdminTaBortAnvändare(Administratör admin, Användare användareAttTaBort)
        {
            ValideraAdmin(admin);

            if (admin.Id == användareAttTaBort.Id)
            {
                throw new Exception("Du kan inte ta bort ditt eget administratörskonto.");
            }

            _databas.TaBortAnvändare(användareAttTaBort);
        }

        /// <summary>
        /// Validerar att den inloggade användaren har administratörsbehörighet.
        /// </summary>
        private void ValideraAdmin(Administratör admin)
        {
            if (admin == null)
            {
                throw new Exception("Behörighet saknas. Endast administratörer kan utföra denna åtgärd.");
            }
        }
    }
}