namespace SportsData.Models
{
    public class CSGOPlayerTeam
    {
        public int PlayerID { get; set; }
        public int TeamID { get; set; }
        public CSGOPlayer Player { get; set; }
        public CSGOTeam Team { get; set; }
    }
}
