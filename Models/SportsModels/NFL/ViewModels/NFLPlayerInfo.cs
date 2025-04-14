using IQSport.Models.NFL.TableModels;

namespace IQSport.Models.SportsModels.NFL.ViewModels
{
    public class NFLPlayerInfo
    {
        public NFLPlayer NFLPlayer { get; set; }
        public string? TeamName { get; set; }

        public NFLPlayerCareerFumbleStat? FumbleStats { get; set; }
        public NFLPlayerCareerPassStat? PassStats { get; set; }
        public NFLPlayerCareerReceiveStat? ReceiveStats { get; set; }
        public NFLPlayerCareerRushStat? RushStats { get; set; }
        public NFLPlayerCareerSackStat? SackStats { get; set; }
        public NFLPlayerCareerKickStat? KickStats { get; set; }


    }
}
