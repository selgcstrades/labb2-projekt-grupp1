namespace Lab2Grupp1GUI
{
    public abstract class Användare
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Epost { get; set; }
        public string Lösenord { get; set; }

        protected Användare(int id, string namn, string epost, string lösenord)
        {
            Id = id;
            Namn = namn;
            Epost = epost;
            Lösenord = lösenord;
        }
    }
}