
namespace IQSport.ViewModels.Premier
{
    public class PremierMatchesViewModel
    {
        public List<MatchViewModel> Matches { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public string CurrentSort { get; set; }
        public string SearchTeam { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
