using SportsData.Models;

namespace SportIQ.Models
{
    public class NFLPlayersStatsViewModel
    {
        public List<NFLPlayerInfo> nflPlayers { get; set; }
        public string? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public int TeamID { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }

    }
}
