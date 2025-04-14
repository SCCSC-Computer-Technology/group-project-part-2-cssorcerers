using IQSport.Data.DbContext;
using IQSport.Models.SportsModels.Premier.Classes;
using Microsoft.EntityFrameworkCore;

namespace IQ_Athlete.Services
{

    public class MatchService
    {
        private readonly SportsDbContext _context;

        public MatchService(SportsDbContext context)
        {
            _context = context;
        }

        public async Task<List<PremierMatch>> GetAllMatchesAsync()
        {
            return await _context.PremierMatch
                .Include(m => m.HomeTeamNavigation)
                .Include(m => m.AwayTeamNavigation)
                .ToListAsync();
        }

        public async Task<List<PremierMatch>> GetSortedMatchesAsync(string sortBy)
        {
            var matches = _context.PremierMatch
                .Include(m => m.HomeTeamNavigation)
                .Include(m => m.AwayTeamNavigation);

            return sortBy switch
            {
                "Team" => await matches.OrderBy(m => m.HomeTeamNavigation.TeamName).ToListAsync(),
                "Goals" => await matches.OrderByDescending(m => m.FullTimeHomeGoals + m.FullTimeAwayGoals).ToListAsync(),
                "Date" => await matches.OrderBy(m => m.Date).ToListAsync(),
                _ => await matches.ToListAsync()
            };
        }

        public async Task<List<PremierMatch>> GetMatchesByTeamNameAsync(string teamName)
        {
            return await _context.PremierMatch
                .Include(m => m.HomeTeamNavigation)
                .Include(m => m.AwayTeamNavigation)
                .Where(m =>
                    m.HomeTeamNavigation.TeamName.Contains(teamName) ||
                    m.AwayTeamNavigation.TeamName.Contains(teamName))
                .ToListAsync();
        }
    }
}