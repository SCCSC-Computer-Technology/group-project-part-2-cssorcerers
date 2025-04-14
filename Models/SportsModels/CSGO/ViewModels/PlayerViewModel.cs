namespace IQSport.Models.SportsModels.CSGO.ViewModels
{
    public class PlayerViewModel
    {
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public int TotalMaps { get; set; }
        public int TotalRounds { get; set; }
        public int KdDiff { get; set; }
        public decimal Kd { get; set; }
        public decimal Rating { get; set; }
        public List<string> TeamNames { get; set; }
    }
}
