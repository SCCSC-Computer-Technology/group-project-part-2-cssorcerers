namespace IQSport.Models.SportsModels.CSGO.Models
{
    public class CSGOPlayerStat
    {
        public int PlayerID { get; set; }
        public int TotalMaps { get; set; }
        public int TotalRounds { get; set; }
        public int KdDiff { get; set; }
        public decimal Kd { get; set; }
        public decimal Rating { get; set; }
        public CSGOPlayer Player { get; set; }
    }
}
