using IQSport.Models.SportsModels.F1.Models;

namespace IQSport.Models.SportsModels.F1.ViewModels
{
    public class F1ConstructorsViewModel
    {
        public List<F1ConstructorInfo> F1Constructor { get; set; }
        public string? ConstructorRef { get; set; }
        public string? Name { get; set; }
        public string? Nationality { get; set; }
        public string? Url { get; set; }
        public string? SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
    }
}
