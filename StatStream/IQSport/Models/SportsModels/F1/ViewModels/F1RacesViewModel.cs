using SportsData.Models;

namespace IQSport.ViewModels.F1
{
    public class F1RacesViewModel
    {
        public List<F1RaceInfo> F1Races { get; set; }
        public int RaceID { get; set; }
        public int? Year { get; set; }
        public int? Round { get; set; }
        public int? CircuitID { get; set; }
        public string? Name { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Url { get; set; }
        public string? SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }

        // Filters if you want to use them later
        public int? YearFilter { get; set; }
        public int? RoundFilter { get; set; }
    }
}
