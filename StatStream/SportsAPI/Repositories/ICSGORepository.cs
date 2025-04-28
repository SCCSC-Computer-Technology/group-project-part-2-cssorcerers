using Microsoft.AspNetCore.Mvc;
using SportsData.Models;

namespace SportsAPI.Interfaces
{
    public interface ICSGORepository
    {
        Task<int> RetrievePlayerCount(string? name, double? minRating, int? minMaps);
        Task<List<CSGOPlayer>> RetrievePlayers(string? name, double? minRating, int? minMaps, int page, int pageSize, string? sort, bool isDesc);

    }
}
