using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SportsAPI.Interfaces;
using SportsData;
using SportsData.Models;
using System.Collections.Concurrent;
using System.Globalization;

namespace SportsAPI.Repositories
{
    public class NBARepository : INBARepository
    {
        private static ConcurrentDictionary<string, NBATeamSeasonInfo> NBATeamCache;
        private static ConcurrentDictionary<string, NBATeamSeasonInfo> NBATeamAvgCache;
        private static ConcurrentDictionary<int, NBAPlayer> NBAPlayerCache;
        private SportsDbContext db;

        public NBARepository(SportsDbContext injectedContext)
        {
            db = injectedContext;


            NBATeamCache = new ConcurrentDictionary<string, NBATeamSeasonInfo>
            (
                db.NBATeam.SelectMany
                (
                    t => t.SeasonStats,
                    (t, s) => new NBATeamSeasonInfo()
                    {
                        ID = t.ID,
                        Name = t.Name,
                        Abreviation = t.Abreviation,
                        TeamStats = s
                    }
                )
                .Distinct()
                .ToDictionary(x => KeyGenerator.GetKey(x.ID, x.TeamStats.Season))
            );
            NBATeamAvgCache = new ConcurrentDictionary<string, NBATeamSeasonInfo>
            (
                db.NBATeam.Select
                    (
                       t => new NBATeamSeasonInfo()
                       {
                           ID = t.ID,
                           Name = t.Name,
                           TeamStats = new NBATeamSeasonStat()
                           {
                               Assists = t.SeasonStats.Select(x => x.Assists).Sum(),
                               Blocks = t.SeasonStats.Select(x => x.Blocks).Sum(),
                               DefesniveRebounds = t.SeasonStats.Select(x => x.DefesniveRebounds).Sum(),
                               FieldGoalAttempts = t.SeasonStats.Select(x => x.FieldGoalAttempts).Sum(),
                               FieldGoals = t.SeasonStats.Select(x => x.FieldGoals).Sum(),
                               FreeThrowAttempts = t.SeasonStats.Select(x => x.FreeThrowAttempts).Sum(),
                               FreeThrows = t.SeasonStats.Select(x => x.FreeThrows).Sum(),
                               OffensiveRebounds = t.SeasonStats.Select(x => x.OffensiveRebounds).Sum(),
                               PersonalFouls = t.SeasonStats.Select(x => x.PersonalFouls).Sum(),
                               Season = -1,
                               Steals = t.SeasonStats.Select(x => x.Steals).Sum(),
                               Team = t,
                               TeamID = t.ID,
                               ThreePointAttempts = t.SeasonStats.Select(x => x.ThreePointAttempts).Sum(),
                               ThreePoints = t.SeasonStats.Select(x => x.ThreePoints).Sum(),
                               TotalPoints = t.SeasonStats.Select(x => x.TotalPoints).Sum(),
                               Turnovers = t.SeasonStats.Select(x => x.Turnovers).Sum(),
                               TwoPointAttempts = t.SeasonStats.Select(x => x.TwoPointAttempts).Sum(),
                               TwoPoints = t.SeasonStats.Select(x => x.TwoPoints).Sum()
                           }
                       }
                    ).Distinct().ToDictionary(x => KeyGenerator.GetKey(x.ID))
            );
            NBAPlayerCache = new ConcurrentDictionary<int, NBAPlayer>
            (
                db.NBAPlayer.Include(x=>x.CareerStats).ToDictionary(x => x.ID)
            );
        }

        public async Task<int> RetrieveMaxSeasonAsync()
        {
            return NBATeamCache.Values.Select(x => x.TeamStats.Season).Max();
        }

        public async Task<int> RetrieveMinSeasonAsync()
        {
            return NBATeamCache.Values.Select(x => x.TeamStats.Season).Min();
        }

        public async Task<List<NBATeamSeasonInfo>> RetrieveAllTeamInfoAsync(string? sortOrderStr, int page)
        {
            if (sortOrderStr == null)
            {
                return NBATeamCache.Values.OrderByDescending(x=>x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NBATeamSeasonInfo> teamInfo = new List<NBATeamSeasonInfo>();
            switch (sortOrderStr)
            {
                case "ssn_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_desc":
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ssn_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_asc":
                    teamInfo = NBATeamCache.Values.OrderBy(x => x.TeamStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    teamInfo = NBATeamCache.Values.OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return teamInfo;
        }

        public async Task<List<NBAPlayer>> RetrieveAllPlayerInfoAsync(string? sortOrderStr, int page)
        {
            if (sortOrderStr == null)
            {
                return NBAPlayerCache.Values.OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NBAPlayer> playerInfo = new List<NBAPlayer>();
            switch (sortOrderStr)
            {
                case "name_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_desc":
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.CareerStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ssn_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_asc":
                    playerInfo = NBAPlayerCache.Values.OrderBy(x => x.CareerStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    playerInfo = NBAPlayerCache.Values.OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return playerInfo;
        }

        public async Task<NBAPlayer> RetrievePlayerInfoByIDAsync(int playerID)
        {
            return NBAPlayerCache[playerID]; 
        }

        public async Task<List<NBAPlayer>> RetrievePlayerInfoByStatusAsync(string? sortOrderStr, bool isActive, int page)
        {
            if (sortOrderStr == null)
            {
                return NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NBAPlayer> playerInfo = new List<NBAPlayer>();
            switch (sortOrderStr)
            {
                case "name_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.CareerStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ssn_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderBy(x => x.CareerStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.IsActive == isActive).OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return playerInfo;
        }

        public async Task<List<NBAPlayer>> RetrievePlayerInfoByTeamAsync(string? sortOrderStr, int teamID, int page)
        {
            if (sortOrderStr == null)
            {
                return NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NBAPlayer> playerInfo = new List<NBAPlayer>();
            switch (sortOrderStr)
            {
                case "name_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_desc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.CareerStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ssn_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_asc":
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderBy(x => x.CareerStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    playerInfo = NBAPlayerCache.Values.Where(x=>x.TeamID == teamID).OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return playerInfo; ;
        }

        public async Task<NBATeamSeasonInfo> RetrieveTeamAverageAsync(int teamID)
        {
            return NBATeamAvgCache[KeyGenerator.GetKey(teamID)];
        }

        public async Task<List<NBATeamSeasonInfo>> RetrieveTeamAveragesAsync(string? sortOrderStr, int page)
        {
            if (sortOrderStr == null)
            {
                return NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NBATeamSeasonInfo> teamInfo = new List<NBATeamSeasonInfo>();
            switch (sortOrderStr)
            {
                case "ssn_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_desc":
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ssn_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_asc":
                    teamInfo = NBATeamAvgCache.Values.OrderBy(x => x.TeamStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    teamInfo = NBATeamAvgCache.Values.OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return teamInfo;
        }

        public async Task<List<NBATeamSeasonInfo>> RetrieveTeamInfoByIDAsync(string? sortOrderStr, int teamID, int page)
        {
            if (sortOrderStr == null)
            {
                return NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NBATeamSeasonInfo> teamInfo = new List<NBATeamSeasonInfo>();
            switch (sortOrderStr)
            {
                case "ssn_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ssn_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderBy(x => x.TeamStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    teamInfo = NBATeamCache.Values.Where(x=>x.ID == teamID).OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return teamInfo;
        }

        public async Task<List<NBATeamSeasonInfo>> RetrieveTeamInfoBySeasonAsync(string? sortOrderStr, int season, int page)
        {
            if (sortOrderStr == null)
            {
                return NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NBATeamSeasonInfo> teamInfo = new List<NBATeamSeasonInfo>();
            switch (sortOrderStr)
            {
                case "ssn_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_desc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ssn_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.FieldGoals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pt_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.ThreePoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "3pta_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.ThreePointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pta_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.TwoPointAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ft_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.FreeThrows).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fta_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.FreeThrowAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "offr_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.OffensiveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "defr_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.DefesniveRebounds).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ast_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.Assists).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "stl_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.Steals).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "blk_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.Blocks).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tnvr_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.Turnovers).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pfl_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.PersonalFouls).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tpt_asc":
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderBy(x => x.TeamStats.TotalPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    teamInfo = NBATeamCache.Values.Where(x=>x.TeamStats.Season == season).OrderByDescending(x => x.TeamStats.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return teamInfo;
        }

        public async Task<NBATeamSeasonInfo> RetrieveTeamSeasonInfoAsync(int teamID, int season)
        {
            return NBATeamCache[KeyGenerator.GetKey(teamID, season)];
        }

        public async Task<int> RetrieveTeamCountAsync(int? season, int? teamID)
        {
            if (teamID.HasValue)
            {
                return NBATeamCache.Values.Where(x=>x.ID == teamID.Value).Count();
            }
            else if (season.HasValue)
            {
                return NBATeamCache.Values.Where(x => x.TeamStats.Season == season.Value).Count();
            }
            else
            {
                return NBATeamCache.Values.Count();
            }
        }

        public async Task<int> RetrievePlayerCountAsync(int? teamID, bool? isActive)
        {
            if(teamID.HasValue)
            {
                return NBAPlayerCache.Values.Where(x => x.TeamID == teamID).Count();
            }
            else if(isActive.HasValue)
            {
                return NBAPlayerCache.Values.Where(x => x.IsActive == isActive.Value).Count();
            }
            else
            {
                return NBAPlayerCache.Values.Count();
            }
        }
    }
}
