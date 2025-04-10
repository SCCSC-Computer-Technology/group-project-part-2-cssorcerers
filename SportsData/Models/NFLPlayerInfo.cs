using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsData.Models.TableModels;

namespace SportsData.Models
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
