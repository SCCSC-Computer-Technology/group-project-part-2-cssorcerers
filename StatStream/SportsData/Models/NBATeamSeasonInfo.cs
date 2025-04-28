

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SportsData.Models
{
    public class NBATeamSeasonInfo
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Abreviation { get; set; }
        public NBATeamSeasonStat TeamStats { get; set; }
    }
}
