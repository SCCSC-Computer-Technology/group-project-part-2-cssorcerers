using System.ComponentModel.DataAnnotations;

namespace IQSport.Models.SportsModels.CSGO.Models
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
