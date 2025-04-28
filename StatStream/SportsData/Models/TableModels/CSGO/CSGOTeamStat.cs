namespace SportsData.Models
{
    public class CSGOTeamStat
    {
        public int TeamID { get; set; }
        public int TotalMaps { get; set; }
        public int KdDiff { get; set; }
        public decimal Kd { get; set; }
        public decimal Rating { get; set; }
        public CSGOTeam Team { get; set; }
    }
}
