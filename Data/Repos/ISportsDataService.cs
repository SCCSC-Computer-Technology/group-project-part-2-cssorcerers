using SportsAPI.Models;

namespace SportsAPI.Data.Repos
{
    public interface ISportsDataService
    {
        IEnumerable<string> GetSports();
        bool IsValidSport(string sport);
        Task<IEnumerable<Highlight>> GetHighlightsAsync(string sport, int page = 1, int size = 10);
        Task<IEnumerable<Schedule>> GetScheduleAsync(string sport, int page = 1, int size = 10);
    }
}
