namespace SportsData.Models
{
    public class F1RaceInfo
    {
        public int RaceID { get; set; }
        public int? Year { get; set; }
        public int? Round { get; set; }
        public int? CircuitID { get; set; }
        public string? Name { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? CircuitName { get; set; }
        public string? Url { get; set; }

        //properties for display formatting
        public string DisplayDate => Date ?? "Unknown";
        public string DisplayTime => Time ?? "TBD";
        public string DisplayName => Name ?? "Unnamed Race";
    }
}
