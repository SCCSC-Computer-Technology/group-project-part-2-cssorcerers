using SportsData.Models;

namespace IQSport.ViewModels.NBA
{
    public class NBAPlayersStatsViewModel
    {
        public List<NBAPlayer> nbaPlayers { get; set; }
        public string? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public int TeamID { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }

    }
}
