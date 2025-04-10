using SportsData.Models.TableModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models
{
    public class NBATeamSeasonInfo
    {
        public NBATeam TeamInfo { get; set; }
        public NBATeamSeasonStat TeamStats { get; set; }
    }
}
