using SportsData.Models;

namespace IQSport.ViewModels.NFL
{
    public class NFLTeamsStatsViewModel
    {
        public List<NFLTeamSeasonInfo> TeamsStats { get; set; }
        public string? SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
        public int Season { get; set; }
    }
}
