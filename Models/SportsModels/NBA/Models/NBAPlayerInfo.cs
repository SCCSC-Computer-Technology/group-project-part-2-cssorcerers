
using IQSport.Models.SportModels.Models;
using IQSport.Models.SportModels.NBA.Models;

namespace IQSport.Models
{
    public class NBAPlayerInfo
    {
        public NBAPlayer NBAPlayer { get; set; }
        public string? TeamName { get; set; }
        public NBAPlayerCareerStat CareerStats { get; set; }
    }
}
