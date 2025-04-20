namespace IQSport.Models.SportsModels.NFL.ViewModels
{
    public class NFLTeamStatsViewModel
    {
        public List<NFLTeamSeasonInfo> TeamsStats { get; set; }
        public string? SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
        public int Season { get; set; }
    }
}
