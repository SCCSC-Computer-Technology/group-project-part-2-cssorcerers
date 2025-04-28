using SportsData.Models;

namespace IQSport.ViewModels.F1
{
    public class F1ConstructorsViewModel
    {
        public List<F1Constructor> F1Constructor { get; set; }
        public string? ConstructorRef { get; set; }
        public string? Name { get; set; }
        public string? Nationality { get; set; }
        public string? Url { get; set; }
        public string? SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
    }
}
