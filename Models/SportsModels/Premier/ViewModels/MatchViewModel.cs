namespace IQSport.Models.SportsModels.Premier.ViewModels
{
    public class MatchViewModel
    {
        public int MatchID { get; set; }
        public DateTime Date { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public int FullTimeHomeGoals { get; set; }
        public int FullTimeAwayGoals { get; set; }
        public string FullTimeResult { get; set; }
        public int? HalfTimeHomeGoals { get; set; }
        public int? HalfTimeAwayGoals { get; set; }
        public string HalfTimeResult { get; set; }
        public string Referee { get; set; }
        public int? HomeShots { get; set; }
        public int? AwayShots { get; set; }
        public int? HomeShotsOnTarget { get; set; }
        public int? AwayShotsOnTarget { get; set; }
        public int? HomeFouls { get; set; }
        public int? AwayFouls { get; set; }
        public int? HomeCorners { get; set; }
        public int? AwayCorners { get; set; }
        public int? HomeYellowCards { get; set; }
        public int? AwayYellowCards { get; set; }
        public int? HomeRedCards { get; set; }
        public int? AwayRedCards { get; set; }
        public string ScoreDisplay { get; set; }
        public string HalfTimeScoreDisplay { get; set; }
    }



}