using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportsAPI.Interfaces;
using SportsData;
using SportsData.Models;
using System.Collections.Concurrent;
using System.Linq;

namespace SportsAPI.Repositories
{
    public class NFLRepository : INFLRepository
    {
        private static ConcurrentDictionary<string, NFLTeamSeasonInfo> NFLTeamCache;
        private static ConcurrentDictionary<string, NFLTeamSeasonInfo> NFLTeamAvgCache;
        private static ConcurrentDictionary<int, NFLPlayer> NFLPlayerCache;
        private SportsDbContext db;

        public NFLRepository(SportsDbContext injectedContext)
        {
            db = injectedContext;


            NFLTeamCache = new ConcurrentDictionary<string, NFLTeamSeasonInfo>
            (
                db.NFLTeam.Include(x=>x.SeasonStats).ThenInclude(x=>x.Team).SelectMany
                (
                    t => t.SeasonStats,
                    (t, s) => new NFLTeamSeasonInfo()
                    {
                        ID = t.ID,
                        Name = t.Name,
                        TeamStat = s
                    }
                )
                .Distinct()
                .ToDictionary(x => KeyGenerator.GetKey(x.ID, x.TeamStat.Season))
            );
            NFLTeamAvgCache = new ConcurrentDictionary<string, NFLTeamSeasonInfo>
            (
                db.NFLTeam.Select
                    (
                       t => new NFLTeamSeasonInfo()
                       {
                           ID = t.ID,
                           Name = t.Name,
                           TeamStat = new NFLTeamSeasonStat()
                           {
                               ExtraPointsMade = t.SeasonStats.Select(x => x.ExtraPointsMade).Sum(),
                               FieldGoalAttempts = t.SeasonStats.Select(x => x.FieldGoalAttempts).Sum(),
                               FieldGoalsMade = t.SeasonStats.Select(x => x.FieldGoalsMade).Sum(),
                               ID = t.SeasonStats.Select(x => x.ID).Sum(),
                               Interceptions = t.SeasonStats.Select(x => x.Interceptions).Sum(),
                               Losses = t.SeasonStats.Select(x => x.Losses).Sum(),
                               PassingYards = t.SeasonStats.Select(x => x.PassingYards).Sum(),
                               ReceivingTouchdowns = t.SeasonStats.Select(x => x.ReceivingTouchdowns).Sum(),
                               RushingTouchdowns = t.SeasonStats.Select(x => x.RushingTouchdowns).Sum(),
                               RushYards = t.SeasonStats.Select(x => x.RushYards).Sum(),
                               Season = -1,
                               Ties = t.SeasonStats.Select(x => x.Ties).Sum(),
                               TotalTouchdowns = t.SeasonStats.Select(x => x.TotalTouchdowns).Sum(),
                               TwoPoints = t.SeasonStats.Select(x => x.TwoPoints).Sum(),
                               Wins = t.SeasonStats.Select(x => x.Wins).Sum()
                           }
                       }
                    ).Distinct().ToDictionary(x => KeyGenerator.GetKey(x.ID))
            );
            NFLPlayerCache = new ConcurrentDictionary<int, NFLPlayer>
            (
                db.NFLPlayer.Include(x=>x.Team).Include(x=>x.FumbleStats).Include(x=>x.KickStats).Include(x=>x.PassStats)
                .Include(x=>x.ReceiveStats).Include(x=>x.RushStats).Include(x=>x.SackStats).ToDictionary(x => x.ID)
            );
        }

        public async Task<List<NFLTeamSeasonInfo>> RetreiveAllNFLTeamInfoAsync(string? sortOrderStr, int page)
        {
            

            if (sortOrderStr == null)
            {
                return NFLTeamCache.Values.OrderBy(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NFLTeamSeasonInfo> teamInfo = new List<NFLTeamSeasonInfo>();
            switch (sortOrderStr)
            {
                case "ssn_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rush_td_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rec_td_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "td_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.TotalTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "xpt_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.ExtraPointsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.FieldGoalsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "win_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.Wins).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "loss_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.Losses).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tie_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.Ties).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ryd_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.RushYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pyd_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.PassingYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "int_desc":
                    teamInfo = NFLTeamCache.Values.OrderByDescending(x => x.TeamStat.Interceptions).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ssn_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rush_td_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.RushingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rec_td_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "td_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.TotalTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "xpt_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.ExtraPointsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.FieldGoalsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "win_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.Wins).Skip(5 * (page -1)).Take(5).ToList(); 
                    break;
                case "loss_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.Losses).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tie_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.Ties).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ryd_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.RushYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pyd_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.PassingYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "int_asc":
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.Interceptions).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    teamInfo = NFLTeamCache.Values.OrderBy(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return teamInfo;
        }

        public async Task<List<NFLPlayer>> RetreiveAllPlayerInfoAsync(string? sortOrderStr, int page)
        {
            if(sortOrderStr == null)
            {
                return NFLPlayerCache.Values.OrderBy(x => x.ID).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NFLPlayer> players = new List<NFLPlayer>();
            switch (sortOrderStr?.ToLower())
            {
                case "name_asc":
                    players = NFLPlayerCache.Values.OrderBy(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "name_desc":
                    players = NFLPlayerCache.Values.OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    players = NFLPlayerCache.Values.OrderBy(x => x.ID).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return players;
        }

        public async Task<int> RetreiveNFLTeamInfoCountAsync(int? season)
        {
            if (!season.HasValue) season = -1;

            return NFLTeamCache.Values.Where(x => x.TeamStat.Season == season.Value).Count();
        }

        public async Task<int> RetreiveMaxSeasonAsync()
        {
            return NFLTeamCache.Values.Select(x => x.TeamStat.Season).Max();
        }

        public async Task<int> RetreiveMinSeasonAsync()
        {
            return NFLTeamCache.Values.Select(x => x.TeamStat.Season).Min();
        }

        public async Task<NFLPlayer> RetreiveNFLPlayerInfoByIDAsync(int playerID)
        {
            return NFLPlayerCache.GetValueOrDefault(playerID);
        }

        public async Task<List<NFLPlayer>> RetreiveNFLPlayerInfoByStatusAsync(string? sortOrderStr, bool isActive, int page)
        {
            if (sortOrderStr == null)
            {
                return NFLPlayerCache.Values.Where(x=> x.IsActive == isActive).OrderBy(x => x.ID).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NFLPlayer> players = new List<NFLPlayer>();
            switch (sortOrderStr?.ToLower())
            {
                case "name_asc":
                    players = NFLPlayerCache.Values.Where(x => x.IsActive == isActive).OrderBy(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "name_desc":
                    players = NFLPlayerCache.Values.Where(x => x.IsActive == isActive).OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    players = NFLPlayerCache.Values.Where(x => x.IsActive == isActive).OrderBy(x => x.ID).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return players;
        }

        public async Task<List<NFLPlayer>> RetreiveNFLPlayerInfoByTeamAsync(string? sortOrderStr, int teamID, int page)
        {
            if (sortOrderStr == null)
            {
                return NFLPlayerCache.Values.Where(x => x.Team?.ID == teamID).OrderBy(x => x.ID).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NFLPlayer> players = new List<NFLPlayer>();
            switch (sortOrderStr?.ToLower())
            {
                case "name_asc":
                    players = NFLPlayerCache.Values.Where(x => x.Team?.ID == teamID).OrderBy(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "name_desc":
                    players = NFLPlayerCache.Values.Where(x => x.Team?.ID == teamID).OrderByDescending(x => x.Name).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    players = NFLPlayerCache.Values.Where(x => x.Team?.ID == teamID).OrderBy(x => x.ID).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return players;
        }
        public async Task<int> RetreiveNFLPlayerCountAsync(int? teamID, bool? isActive)
        {
            if(teamID.HasValue)
            {
                return NFLPlayerCache.Values.Where(x=>x.Team?.ID == teamID).Count();
            }
            else if(isActive.HasValue)
            {
                return NFLPlayerCache.Values.Where(x => x.IsActive == isActive.Value).Count();
            }
            else
            {
                return NFLPlayerCache.Values.Count();
            }
        }
        public async Task<NFLTeamSeasonInfo> RetreiveNFLTeamAverageAsync(int teamID)
        {
            return NFLTeamAvgCache.Values.Where(x => x.ID == teamID).SingleOrDefault();
        }

        public async Task<List<NFLTeamSeasonInfo>> RetreiveNFLTeamAveragesAsync(string? sortOrderStr, int page)
        {
            if (sortOrderStr == null)
            {
                return NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NFLTeamSeasonInfo> teamInfo = new List<NFLTeamSeasonInfo>();
            switch (sortOrderStr)
            {
                case "ssn_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rush_td_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rec_td_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "td_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.TotalTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "xpt_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.ExtraPointsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.FieldGoalsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "win_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.Wins).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "loss_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.Losses).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tie_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.Ties).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ryd_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.RushYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pyd_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.PassingYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "int_desc":
                    teamInfo = NFLTeamAvgCache.Values.OrderByDescending(x => x.TeamStat.Interceptions).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ssn_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rush_td_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.RushingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rec_td_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "td_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.TotalTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "xpt_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.ExtraPointsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.FieldGoalsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "win_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.Wins).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "loss_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.Losses).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tie_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.Ties).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ryd_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.RushYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pyd_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.PassingYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "int_asc":
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.Interceptions).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    teamInfo = NFLTeamAvgCache.Values.OrderBy(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return teamInfo;
        }

        public async Task<List<NFLTeamSeasonInfo>> RetreiveNFLTeamInfoByIDAsync(string? sortOrderStr, int teamID, int page)
        {
            if (sortOrderStr == null)
            {
                return NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NFLTeamSeasonInfo> teamInfo = new List<NFLTeamSeasonInfo>();
            switch (sortOrderStr)
            {
                case "ssn_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rush_td_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rec_td_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "td_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.TotalTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "xpt_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.ExtraPointsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.FieldGoalsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "win_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.Wins).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "loss_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.Losses).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tie_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.Ties).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ryd_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.RushYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pyd_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.PassingYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "int_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderByDescending(x => x.TeamStat.Interceptions).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ssn_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rush_td_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.RushingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rec_td_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "td_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.TotalTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "xpt_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.ExtraPointsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.FieldGoalsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "win_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.Wins).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "loss_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.Losses).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tie_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.Ties).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ryd_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.RushYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pyd_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.PassingYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "int_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.Interceptions).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    teamInfo = NFLTeamCache.Values.Where(x => x.ID == teamID).OrderBy(x => x.TeamStat.Season).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return teamInfo;
        }

        public async Task<List<NFLTeamSeasonInfo>> RetreiveNFLTeamInfoBySeasonAsync(string? sortOrderStr, int season, int page)
        {
            if (sortOrderStr == null)
            {
                return NFLTeamCache.Values.Where(x=>x.TeamStat.Season == season).OrderBy(x => x.ID).Skip(5 * (page -1)).Take(5).ToList();
            }

            List<NFLTeamSeasonInfo> teamInfo = new List<NFLTeamSeasonInfo>();
            switch (sortOrderStr)
            {
                case "rush_td_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rec_td_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "td_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.TotalTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "xpt_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.ExtraPointsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.FieldGoalsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "win_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.Wins).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "loss_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.Losses).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tie_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.Ties).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ryd_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.RushYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pyd_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.PassingYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "int_desc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderByDescending(x => x.TeamStat.Interceptions).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rush_td_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.RushingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "rec_td_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.ReceivingTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "td_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.TotalTouchdowns).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.TwoPoints).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "xpt_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.ExtraPointsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fg_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.FieldGoalsMade).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "fga_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.FieldGoalAttempts).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "win_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.Wins).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "loss_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.Losses).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "tie_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.Ties).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "ryd_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.RushYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "pyd_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.PassingYards).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                case "int_asc":
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.TeamStat.Interceptions).Skip(5 * (page -1)).Take(5).ToList();
                    break;
                default:
                    teamInfo = NFLTeamCache.Values.Where(x => x.TeamStat.Season == season).OrderBy(x => x.ID).Skip(5 * (page -1)).Take(5).ToList();
                    break;
            }
            return teamInfo;
        }

        public async Task<NFLTeamSeasonInfo> RetreiveNFLTeamSeasonInfoAsync(int teamID, int season)
        {
            return NFLTeamCache.Values.Where(x => x.ID == teamID && x.TeamStat.Season == season).SingleOrDefault();
        }
        
    }
}
