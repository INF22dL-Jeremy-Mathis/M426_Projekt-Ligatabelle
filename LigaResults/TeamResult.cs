namespace LigaResults
{
    public class TeamResult
    {
        public required string Name { get; set; }
        public int Siege { get; set; }
        public int Niederlagen { get; set; }
        public int Unentschieden { get; set; }
        public int Tore { get; set; }
        public int Gegentore { get; set; }
        public int Tordifferenz { get; set; }
        public int Punkte { get; set; }
        public int Rang { get; set; }
    }
}
