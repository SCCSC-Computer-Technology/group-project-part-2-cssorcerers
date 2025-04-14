using IQSport.Data.DbContext;
using IQSport.Models.SportsModels.CSGO.ViewModels;
using IQSport.Models.SportsModels.Premier.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace IQSport.Data.Services
{
    public interface ICSGOService
    {
        Task<(List<PlayerViewModel> Players, PaginationViewModel Pagination)> GetPlayers(PlayerFilterViewModel filter);
    }

    public class CsgoService : ICSGOService
    {
        private readonly SportsDbContext _context;

        public CsgoService(SportsDbContext context)
        {
            _context = context;
        }

        public async Task<(List<PlayerViewModel> Players, PaginationViewModel Pagination)> GetPlayers(PlayerFilterViewModel filter)
        {
            var query = from p in _context.CSGOPlayers
                        join ps in _context.CSGOPlayerStats on p.PlayerID equals ps.PlayerID
                        select new PlayerViewModel
                        {
                            PlayerID = p.PlayerID,
                            PlayerName = p.PlayerName,
                            TotalMaps = ps.TotalMaps,
                            TotalRounds = ps.TotalRounds,
                            KdDiff = ps.KdDiff,
                            Kd = ps.Kd,
                            Rating = ps.Rating,
                            TeamNames = (from pt in _context.CSGOPlayerTeams
                                         join t in _context.CSGOTeams on pt.TeamID equals t.TeamID
                                         where pt.PlayerID == p.PlayerID
                                         select t.TeamName).ToList()
                        };

            // Apply filters
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(p => p.PlayerName.Contains(filter.SearchTerm));
            }

            if (filter.MinRating.HasValue)
            {
                query = query.Where(p => p.Rating >= filter.MinRating.Value);
            }

            if (filter.MinMaps.HasValue)
            {
                query = query.Where(p => p.TotalMaps >= filter.MinMaps.Value);
            }

            // Apply sorting
            query = filter.SortBy switch
            {
                "Rating" => filter.SortDescending ?
                    query.OrderByDescending(p => p.Rating) :
                    query.OrderBy(p => p.Rating),
                "Kd" => filter.SortDescending ?
                    query.OrderByDescending(p => p.Kd) :
                    query.OrderBy(p => p.Kd),
                "Maps" => filter.SortDescending ?
                    query.OrderByDescending(p => p.TotalMaps) :
                    query.OrderBy(p => p.TotalMaps),
                _ => filter.SortDescending ?
                    query.OrderByDescending(p => p.PlayerName) :
                    query.OrderBy(p => p.PlayerName)
            };

            // Calculate pagination
            int pageSize = filter.PageSize > 0 ? filter.PageSize : 10; // Default to 10 if not set
            int currentPage = filter.CurrentPage > 0 ? filter.CurrentPage : 1; // Default to 1
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Apply pagination
            var paginatedQuery = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            var players = await paginatedQuery.ToListAsync();

            var pagination = new PaginationViewModel
            {
                CurrentPage = currentPage,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return (players, pagination);
        }
    }
}