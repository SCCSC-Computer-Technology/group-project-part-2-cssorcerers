namespace IQSport.Models.SportsModels.NBA.ViewModels
{
    public class NBATeamsStatsViewModel
    {
        public List<NBATeamSeasonInfo> TeamsStats { get; set; }
        public string? SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
        public int Season { get; set; }
    }
}
