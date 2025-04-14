namespace IQSport.Models.SportsModels.NFL.ViewModels
{
    public class NFLTeamSeasonInfo
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int Season { get; set; }
        public int RushingTouchdowns { get; set; }
        public int ReceivingTouchdowns { get; set; }
        public int Touchdowns { get; set; }
        public int ExtraPoints { get; set; }
        public int TwoPoints { get; set; }
        public int FieldGoals { get; set; }
        public int FieldGoalAttempts { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }
        public int RushingYards { get; set; }
        public int PassingYards { get; set; }
        public int Intereceptions { get; set; }


    }
}
