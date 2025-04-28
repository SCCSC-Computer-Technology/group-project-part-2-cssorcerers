using SportsData.Models;

namespace SportsAPI.Interfaces
{
    public interface IPremierRepository
    {

        Task<List<PremierMatch>> RetrievePremierMatchesAsync(string? sortOrderStr, int page, int itemsPerPage,
                string? searchTerm, DateTime? dateFrom, DateTime? dateTo);
        Task<PremierTeam> RetrievePremierTeamAsync(int id);

        //ideally, this will get the id from the matching team and return RetreivePremierMatchByTeamIDAsync
        Task<List<PremierMatch>> RetrievePremierMatchesByTeamNameAsync(string? sortOrderStr, string teamName, int page);

        Task<List<PremierMatch>> RetrievePremierMatchByTeamIDAsync(string? sortOrderStr, int id, int page);

        Task<List<PremierMatch>> RetrievePremierMatchesByDateAsync(string? sortOrderStr, DateTime date, int page);

        Task<int> RetrievePremierMatchesCountAsync(string? searchTerm, DateTime? dateFrom, DateTime? dateTo);

        Task<DateTime> RetrieveMaxDateAsync();
        Task<DateTime> RetrieveMinDateAsync();
    }
}
