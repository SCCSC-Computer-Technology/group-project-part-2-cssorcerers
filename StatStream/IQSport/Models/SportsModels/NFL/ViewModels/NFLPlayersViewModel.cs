using SportsData.Models;

namespace IQSport.ViewModels.NFL
{
    public class NFLPlayersStatsViewModel
    {
        public List<NFLPlayer> nflPlayers { get; set; }
        public string? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public int TeamID { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }

    }
}
