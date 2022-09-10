namespace timeliste.Models
{
    public class Prosjekt
    {
        public int Id { get; set; }
        public string ProsjektNavn { get; set; }
        public string KundeNavn { get; set; }
        public string Info { get; set; }
        public int PeriodeLengde { get; set; }
    }
}