namespace IQSport.Models.SportsModels.CSGO.ViewModels
{
    public class PlayerFilterViewModel : IQSport.Models.SportsModels.Premier.ViewModels.PaginationViewModel
    {
        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int? MinRating { get; set; }
        public int? MinMaps { get; set; }
    }
}