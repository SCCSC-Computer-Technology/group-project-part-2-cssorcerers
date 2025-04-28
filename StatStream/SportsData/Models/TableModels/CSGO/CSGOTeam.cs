using System.Text.Json.Serialization;

namespace SportsData.Models
{
    public class CSGOTeam
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public CSGOTeamStat TeamStat { get; set; }

        [JsonIgnore]
        public List<CSGOPlayerTeam> TeamPlayers { get; set; }
    }
}
