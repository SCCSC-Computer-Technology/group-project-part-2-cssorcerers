using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SportsData.Models
{
    public class CSGOPlayer
    {
        [Key]
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public CSGOPlayerStat PlayerStat { get; set; }
        public List<CSGOPlayerTeam> PlayerTeams { get; set; }
    }
}
