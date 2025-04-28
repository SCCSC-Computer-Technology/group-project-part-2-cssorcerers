

namespace SportsData.Models
{
    public class NBAPlayerInfo
    {
        public NBAPlayer NBAPlayer { get; set; }
        public string? TeamName { get; set; }
        public NBAPlayerCareerStat CareerStats { get; set; }
    }
}
