using SportsData.Models.TableModels;
using SportsData.Models.TableModels.NBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models
{
    public class NBAPlayerInfo
    {
        public NBAPlayer NBAPlayer { get; set; }
        public string? TeamName { get; set; }
        public NBAPlayerCareerStat CareerStats { get; set; }
    }
}
