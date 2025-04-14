using IQSport.Models;

namespace IQSports.Models.SportModels.NBA.ViewModels
{
    public class NBAPlayersStatsViewModel
    {
        public List<NBAPlayerInfo> nbaPlayers { get; set; }
        public string? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public int TeamID { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }

    }
}
