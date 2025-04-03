using Microsoft.EntityFrameworkCore;
using SportsData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsData.Models;
using SportsData.Models.TableModels;

namespace SportsData.Services
{
    public class NFLServices
    {
        public static async Task<List<NFLTeamSeasonInfo>> GetNFLTeamSeasonInfoList(int season, string? sortOrderStr, int page,
            int itemsPerPage, List<NFLTeamSeasonStat> teamSeasonStats, SportsDbContext context)
        {
           

            List<NFLTeam> teams = await context.NFLTeam.Where(x => teamSeasonStats.Select(y => y.ID).Contains(x.ID)).OrderBy(x => x.ID).ToListAsync();

            List<NFLTeamSeasonInfo> teamInfo = new List<NFLTeamSeasonInfo>();
            foreach (NFLTeam team in teams)
            {
                
                var seasonStat = teamSeasonStats.Where(stat => stat.ID == team.ID && (season == -1 || stat.Season == season)).ToList();
                
                foreach (NFLTeamSeasonStat stat in seasonStat)
                {
                    teamInfo.Add(new NFLTeamSeasonInfo
                    {
                        TeamID = team.ID,
                        TeamName = team.Name,
                        Season = stat.Season,
                        RushingTouchdowns = stat.RushingTouchdowns,
                        ReceivingTouchdowns = stat.ReceivingTouchdowns,
                        Touchdowns = stat.TotalTouchdowns,
                        ExtraPoints = stat.ExtraPointsMade,
                        TwoPoints = stat.TwoPoints,
                        FieldGoals = stat.FieldGoalsMade,
                        FieldGoalAttempts = stat.FieldGoalAttempts,
                        Wins = stat.Wins,
                        Losses = stat.Losses,
                        Ties = stat.Ties,
                        RushingYards = stat.RushYards,
                        PassingYards = stat.PassingYards,
                        Intereceptions = stat.Interceptions
                    });
                }
            }

            switch (sortOrderStr)
            {
                case "ssn_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Season).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "rush_td_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.RushingTouchdowns).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "rec_td_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.ReceivingTouchdowns).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "td_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Touchdowns).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pt_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.TwoPoints).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "xpt_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.ExtraPoints).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fg_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.FieldGoals).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fga_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.FieldGoalAttempts).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "win_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Wins).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "loss_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Losses).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tie_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Ties).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ryd_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.RushingYards).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "pyd_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.PassingYards).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "int_desc":
                    teamInfo = teamInfo.OrderByDescending(x => x.Intereceptions).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ssn_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Season).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "rush_td_asc":
                    teamInfo = teamInfo.OrderBy(x => x.RushingTouchdowns).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "rec_td_asc":
                    teamInfo = teamInfo.OrderBy(x => x.ReceivingTouchdowns).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "td_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Touchdowns).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "2pt_asc":
                    teamInfo = teamInfo.OrderBy(x => x.TwoPoints).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "xpt_asc":
                    teamInfo = teamInfo.OrderBy(x => x.ExtraPoints).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fg_asc":
                    teamInfo = teamInfo.OrderBy(x => x.FieldGoals).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "fga_asc":
                    teamInfo = teamInfo.OrderBy(x => x.FieldGoalAttempts).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "win_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Wins).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "loss_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Losses).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "tie_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Ties).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "ryd_asc":
                    teamInfo = teamInfo.OrderBy(x => x.RushingYards).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "pyd_asc":
                    teamInfo = teamInfo.OrderBy(x => x.PassingYards).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case "int_asc":
                    teamInfo = teamInfo.OrderBy(x => x.Intereceptions).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                default:
                    teamInfo = teamInfo.OrderByDescending(x => x.Season).ThenBy(x => x.TeamID).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
            }
            return teamInfo;
        }

        public static async Task<NFLTeamSeasonInfo> GetNFLTeamSeasonInfo(int teamID, int season, SportsDbContext context)
        {
            if (season == -1)
            {
                var stats = context.NFLTeamSeasonStat.Where(x => x.ID == teamID);
                if (stats.Any())
                {
                    return new NFLTeamSeasonInfo()
                    {
                        TeamID = teamID,
                        TeamName = context.NFLTeam.Where(x => x.ID == teamID).FirstOrDefault().Name,
                        Season = -1,
                        RushingTouchdowns = stats.Select(x => x.RushingTouchdowns).Sum(),
                        ReceivingTouchdowns = stats.Select(x => x.ReceivingTouchdowns).Sum(),
                        Touchdowns = stats.Select(x => x.TotalTouchdowns).Sum(),
                        ExtraPoints = stats.Select(x => x.ExtraPointsMade).Sum(),
                        TwoPoints = stats.Select(x => x.TwoPoints).Sum(),
                        FieldGoals = stats.Select(x => x.FieldGoalsMade).Sum(),
                        FieldGoalAttempts = stats.Select(x => x.FieldGoalAttempts).Sum(),
                        Wins = stats.Select(x => x.Wins).Sum(),
                        Losses = stats.Select(x => x.Losses).Sum(),
                        Ties = stats.Select(x => x.Ties).Sum(),
                        RushingYards = stats.Select(x => x.RushYards).Sum(),
                        PassingYards = stats.Select(x => x.PassingYards).Sum(),
                        Intereceptions = stats.Select(x => x.Interceptions).Sum()
                    };
                }
            }
            else
            {
                if (context.NFLTeamSeasonStat.Any(x => x.Season == season && x.ID == teamID))
                {
                    var stats = context.NFLTeamSeasonStat.Where(x=>x.ID == teamID && x.Season == season).FirstOrDefault();
                    return new NFLTeamSeasonInfo()
                    {
                        TeamID = teamID,
                        TeamName = context.NFLTeam.Where(x => x.ID == teamID).FirstOrDefault().Name,
                        Season = season,
                        RushingTouchdowns = stats.RushingTouchdowns,
                        ReceivingTouchdowns = stats.ReceivingTouchdowns,
                        Touchdowns = stats.TotalTouchdowns,
                        ExtraPoints = stats.ExtraPointsMade,
                        TwoPoints = stats.TwoPoints,
                        FieldGoals = stats.FieldGoalsMade,
                        FieldGoalAttempts = stats.FieldGoalAttempts,
                        Wins = stats.Wins,
                        Losses = stats.Losses,
                        Ties = stats.Ties,
                        RushingYards = stats.RushYards,
                        PassingYards = stats.PassingYards,
                        Intereceptions = stats.Interceptions
                    };
                }
            }
            throw new Exception($"Could not find data for team ID {teamID}, season {season} in the database");
        }
    

        public static async Task<List<NFLPlayerInfo>> GetNFLPlayerInfoList(bool? isActive, int teamID,string? sortOrderStr, int page,
            int itemsPerPage,  SportsDbContext context)
        {

            IQueryable<NFLPlayer> players;
            if (isActive == null)
            {
                switch (sortOrderStr)
                {
                    case "name_asc":
                        players = context.NFLPlayer.Where(x => (teamID == -1 ? true : x.TeamID == teamID))
                            .OrderBy(x => x.Name).ThenBy(x => x.ID).Skip(itemsPerPage * (page-1)).Take(itemsPerPage);
                        break;
                    case "name_desc":
                        players = context.NFLPlayer.Where(x => (teamID == -1 ? true : x.TeamID == teamID))
                            .OrderByDescending(x => x.Name).ThenByDescending(x => x.ID)
                            .Skip(itemsPerPage * (page - 1)).Take(itemsPerPage);
                        break;
                    default:
                        players = context.NFLPlayer.Where(x => (teamID == -1 ? true : x.TeamID == teamID))
                            .OrderBy(x => x.ID).Skip(itemsPerPage * (page - 1)).Take(itemsPerPage);
                        break;
                }
            }
            else
            {
                switch (sortOrderStr.ToLower())
                {
                    case "name_asc":
                        players = context.NFLPlayer.Where(x => x.IsActive == isActive && (teamID == -1 ? true : x.TeamID == teamID))
                        .OrderBy(x => x.Name).ThenBy(x => x.ID).Skip(itemsPerPage * (page - 1)).Take(itemsPerPage);
                        break;
                    case "name_desc":
                    players = context.NFLPlayer.Where(x => x.IsActive == isActive && (teamID == -1 ? true : x.TeamID == teamID))
                            .OrderByDescending(x => x.Name).ThenByDescending(x => x.ID)
                            .Skip(itemsPerPage * (page - 1)).Take(itemsPerPage);
                        break;
                    default:
                    players = context.NFLPlayer.Where(x => x.IsActive == isActive && (teamID == -1 ? true : x.TeamID == teamID))
                            .OrderBy(x => x.ID).Skip(itemsPerPage * (page - 1)).Take(itemsPerPage);
                        break;
                }
            }


            List<NFLPlayerInfo> playerInfo = new List<NFLPlayerInfo>();
            string[] teams = context.NFLTeam.Select(x=>x.Name).ToArray();
            foreach(NFLPlayer player in players)
            {

                playerInfo.Add(new NFLPlayerInfo()
                {
                    ID = player.ID,
                    Name = player.Name,
                    TeamID = player.TeamID,
                    IsActive = player.IsActive,
                    TeamName = (player.TeamID.HasValue) ? teams[player.TeamID.Value - 1] : null
                });
            }

            return playerInfo;
        }
    }
}
