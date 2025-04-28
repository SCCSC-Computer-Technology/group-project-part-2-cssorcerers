using SportsData.Models;

namespace IQSport.ViewModels.NFL
{
    public class NFLTeamStatsViewModel
    {
        public List<NFLTeamSeasonStat> TeamsStats { get; set; }
        public string? SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
        public int Season { get; set; }
    }
}
