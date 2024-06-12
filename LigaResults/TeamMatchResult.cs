namespace LigaResults
{
    public class TeamMatchResult
    {
        public required string TeamName { get; set; }
        public int GoalsShot { get; set; }
        public int GoalsTaken { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
    }
}
