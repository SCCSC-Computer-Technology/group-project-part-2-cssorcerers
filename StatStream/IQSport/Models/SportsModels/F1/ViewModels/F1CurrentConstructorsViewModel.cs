using SportsData.Models;

namespace IQSport.ViewModels.F1
{
    public class F1CurrentConstructorsViewModel
    {
        public List<F1CurrentConstructor> F1CurrentConstructor { get; set; }
        public int ConstructorID { get; set; }
        public string? TeamName { get; set; }
        public string? Principal { get; set; }
        public string? Driver1 { get; set; }
        public int? D1Num { get; set; }
        public string? Driver2 { get; set; }
        public int? D2Num { get; set; }
        public int? Championships { get; set; }
        public string? Base { get; set; }
        public string? PowerUnit { get; set; }
        public string? SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
    }
}
