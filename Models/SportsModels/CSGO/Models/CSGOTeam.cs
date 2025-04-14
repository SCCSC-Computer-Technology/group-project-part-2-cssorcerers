namespace IQSport.Models.SportsModels.CSGO.Models
{
    public class CSGOTeam
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public CSGOTeamStat TeamStat { get; set; }
        public List<CSGOPlayerTeam> TeamPlayers { get; set; }
    }
}
